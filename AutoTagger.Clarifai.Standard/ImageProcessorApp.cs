using System;

namespace AutoTagger.ImageProcessor.Standard
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    using AutoTagger.Contract;

    using Google.Cloud.Vision.V1;

    public class ImageProcessorApp
    {
        private static IImageProcessorStorage storage;
        private static ITaggingProvider tagger;
        private static ConcurrentQueue<IImage> queue;
        private static ConcurrentQueue<IImage> dbSaveQueue;
        public static Action<IImage> OnLookingForTags;
        public static Action<IImage> OnFoundTags;
        public static Action<IImage> OnDbInserted;
        public static Action OnDbSleep;
        public static Action OnDbSaved;
        private static int taggerRunning;
        private static int saveCounter;
        private static readonly Random Random = new Random();

        private static readonly int ConcurrentThreadsLimit = 15;
        private static readonly int SaveLimit = 5;
        private static readonly string PathUnused = @"C:\Instagger\Unused\";
        private static readonly string PathUsed = @"C:\Instagger\Used\";
        private static readonly string Ext = ".jpg";

        public ImageProcessorApp(IImageProcessorStorage db, ITaggingProvider taggingProvider)
        {
            storage = db;
            tagger = taggingProvider;
            dbSaveQueue = new ConcurrentQueue<IImage>();
            queue = new ConcurrentQueue<IImage>();
        }

        public void Process()
        {
            this.PrepareImages();
            new Thread(ImageProcessorApp.StartTaggerThreads).Start();
            new Thread(ImageProcessorApp.InsertDb).Start();
        }

        private void PrepareImages()
        {
            var files = this.GetImagesFromDisk();
            var images = this.GetImagesWithoutMTags(files);
            images.ForEach(i => queue.Enqueue(i));
            this.MoveUsedFiles(files, images);
        }

        private string[] GetImagesFromDisk()
        {
            var files = Directory.GetFiles(PathUnused, "*"+ Ext);
            return files.Select(x => x.Replace(PathUnused, "").Replace(Ext, "")).ToArray();
        }

        private IEnumerable<IImage> GetImagesWithoutMTags(string[] files)
        {
            return storage.GetImagesWithoutMachineTags(files.ToList());
        }

        private void MoveUsedFiles(string[] files, IEnumerable<IImage> images)
        {
            var usedFiles = files.Where(x => !images.Any(y => y.Shortcode == x)).ToList();
            foreach (var usedFile in usedFiles)
            {
                var path     = PathUnused + usedFile + Ext;
                var pathUsed = PathUsed + usedFile + Ext;
                File.Move(path, pathUsed);
            }
        }

        private static void StartTaggerThreads()
        {
            while (true)
            {
                for (var i = taggerRunning; i < ConcurrentThreadsLimit;i++)
                {
                    if(queue.TryDequeue(out IImage image))
                    {
                        new Thread(DoTaggerRequest).Start(image);
                    }
                    else
                    {
                        break;
                    }
                }
                Thread.Sleep(Random.Next(50, 150));
            }
        }

        public static void DoTaggerRequest(object data)
        {
            var image = (IImage)data;
            Interlocked.Increment(ref taggerRunning);
            OnLookingForTags?.Invoke(image);

            var path = PathUnused + image.Shortcode + Ext;
            var mTags = tagger.GetTagsForFile(path).ToList();

            Interlocked.Decrement(ref taggerRunning);
            if (mTags.Count == 0)
                return;
            image.MachineTags = mTags;
            dbSaveQueue.Enqueue(image);
            Interlocked.Increment(ref saveCounter);
            OnFoundTags?.Invoke(image);

            var pathUsed = PathUsed + image.Shortcode + Ext;
            File.Move(path, pathUsed);
        }

        public static void InsertDb()
        {
            while (true)
            {
                if (saveCounter >= SaveLimit)
                {
                    while (dbSaveQueue.TryDequeue(out IImage image))
                    {
                        storage.InsertMachineTagsWithoutSaving(image);
                        OnDbInserted?.Invoke(image);
                    }
                    storage.DoSave();
                    OnDbSaved?.Invoke();
                    saveCounter = 0;
                }
                else
                {
                    OnDbSleep?.Invoke();
                    var r = new Random();
                    Thread.Sleep(r.Next(50, 150));
                }
            }
        }
    }
}
