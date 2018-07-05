namespace AutoTagger.ImageProcessor.Standard
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using AutoTagger.Contract;
    using AutoTagger.FileHandling.Standard;

    public class ImageProcessorApp
    {
        public static Action<IImage> OnDbInserted;

        public static Action OnDbSaved;

        public static Action OnDbSleep;

        public static Action<IImage> OnFoundTags;

        public static Action<IImage> OnLookingForTags;

        private static readonly int ConcurrentThreadsLimit = 15;

        private static readonly Random Random = new Random();

        private static readonly int SaveLimit = 5;

        private static DbUsage currentDbUsage;

        private static ConcurrentQueue<IImage> dbSaveQueue;

        private static IFileHandler fileHandler;

        private static List<string> processed;

        private static ConcurrentQueue<IImage> queue;

        private static int saveCounter;

        private static IImageProcessorStorage storage;

        private static ITaggingProvider tagger;

        private static int taggerRunning;

        public ImageProcessorApp(IImageProcessorStorage db, ITaggingProvider taggingProvider)
        {
            storage     = db;
            tagger      = taggingProvider;
            dbSaveQueue = new ConcurrentQueue<IImage>();
            queue       = new ConcurrentQueue<IImage>();
            processed   = new List<string>();
            fileHandler = new DiskFileHander();
        }

        enum DbUsage
        {
            None,
            GetEntries,
            SaveThisFuckingShit
        }

        public static void DoTaggerRequest(object data)
        {
            var image = (IImage) data;
            Interlocked.Increment(ref taggerRunning);
            OnLookingForTags?.Invoke(image);

            //if (!fileHandler.FileExists(image.Shortcode))
            //{
            //    Interlocked.Decrement(ref taggerRunning);
            //    return;
            //}

            if (fileHandler.GetFileSize(image.Shortcode) == 0)
            {
                Interlocked.Decrement(ref taggerRunning);
                Console.WriteLine("Defect file (Size: 0 Bytes): " + image.Shortcode);
                fileHandler.FlagAsDefect(image.Shortcode);
                return;
            }

            try
            {
                var path  = fileHandler.GetFullPath(image.Shortcode);
                var mTags = tagger.GetTagsForFile(path).ToList();
                Interlocked.Decrement(ref taggerRunning);
                if (!mTags.Any())
                {
                    Console.WriteLine("Image not detectable: " + image.Shortcode);
                    fileHandler.Delete(image.Shortcode);
                    return;
                }

                image.MachineTags = mTags;
                dbSaveQueue.Enqueue(image);
                Interlocked.Increment(ref saveCounter);
                OnFoundTags?.Invoke(image);

                fileHandler.FlagAsUsed(image.Shortcode);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Bad image data"))
                {
                    fileHandler.FlagAsDefect(image.Shortcode);
                    Console.WriteLine("Defect file (Bad image data): " + image.Shortcode);
                    Interlocked.Decrement(ref taggerRunning);
                }
                else
                {
                    Console.WriteLine("Unknown Error");
                }
            }
        }

        public static void InsertDb()
        {
            while (true)
            {
                if (saveCounter >= SaveLimit && SetDbUsing(DbUsage.SaveThisFuckingShit))
                {
                    while (dbSaveQueue.TryDequeue(out IImage image))
                    {
                        storage.InsertMachineTagsWithoutSaving(image);
                        OnDbInserted?.Invoke(image);
                    }

                    storage.Save();
                    OnDbSaved?.Invoke();
                    saveCounter    = 0;
                    currentDbUsage = DbUsage.None;
                }
                else
                {
                    OnDbSleep?.Invoke();
                    var r = new Random();
                    Thread.Sleep(r.Next(50, 150));
                }
            }
        }

        public void Process()
        {
            new Thread(StartTaggerThreads).Start();
            new Thread(InsertDb).Start();
        }

        private static IEnumerable<IImage> GetImagesWithoutMTags(IList<string> files)
        {
            return storage.GetImagesWithoutMachineTags(files);
        }

        private static void PrepareImages()
        {
            var files  = fileHandler.GetAllUnusedImages();
            var images = GetImagesWithoutMTags(files);
            images.ForEach(
                i =>
                {
                    if (processed.Contains(i.Shortcode))
                        return;
                    queue.Enqueue(i);
                    processed.Add(i.Shortcode);
                });

            // bad performance -> better would be to run the Query other way
            //this.MoveUsedFiles(files, images);
        }

        private static bool SetDbUsing(DbUsage newStatus)
        {
            if (currentDbUsage == DbUsage.None || currentDbUsage == newStatus)
            {
                currentDbUsage = newStatus;
                if (currentDbUsage == newStatus)
                    return true;
            }

            return false;
        }

        //private void MoveUsedFiles(string[] files, IEnumerable<IImage> images)
        //{
        //    var usedFiles = files.Where(x => !images.Any(y => y.Shortcode == x)).ToList();
        //    foreach (var usedFile in usedFiles)
        //    {
        //        var path     = PathUnused + usedFile + Ext;
        //        var pathUsed = PathUsed + usedFile + Ext;
        //        File.Move(path, pathUsed);
        //    }
        //}

        private static void StartTaggerThreads()
        {
            while (true)
            {
                for (var i = taggerRunning; i < ConcurrentThreadsLimit; i++)
                {
                    if (queue.TryDequeue(out IImage image))
                    {
                        new Thread(DoTaggerRequest).Start(image);
                    }
                    else
                    {
                        if (SetDbUsing(DbUsage.GetEntries))
                        {
                            PrepareImages();
                            currentDbUsage = DbUsage.None;
                        }

                        break;
                    }
                }

                Thread.Sleep(Random.Next(50, 150));
            }
        }
    }
}
