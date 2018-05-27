namespace AutoTagger.ImageDownloader.Standard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Standard;
    using AutoTagger.FileHandling.Standard;

    public class ImageDownloader
    {
        private static IImageProcessorStorage storage;
        private static int downloaderRunning;

        private static readonly int QueryImagesAtLessOrEqualImages = 20;
        private static readonly int DbSelectImagesAmount = 500;
        private static readonly int ParallelThreads = 100;
        private static int lastId = 0;
        private static IFileHandler fileHandler;
        private static List<string> files;

        public ImageDownloader(IImageProcessorStorage db)
        {
            storage = db;
            fileHandler = new DiskFileHander();
        }

        public void Start()
        {
            files = fileHandler.GetAllUnusedImages().ToList();
            lastId = storage.GetLargestPhotoIdForPhotoWithMTag();
            new Thread(ImageDownloader.GetImages).Start();
        }

        public static void GetImages()
        {
            while (true)
            {
                if (downloaderRunning <= QueryImagesAtLessOrEqualImages)
                {
                    Console.WriteLine("Check DB for ID=" + lastId);
                    var images = storage.GetImagesWithoutMachineTags(lastId, DbSelectImagesAmount);
                    foreach (var image in images)
                    {
                        lastId = image.Id;
                        if (files.Contains(image.Shortcode))
                            continue;
                        if (downloaderRunning >= ParallelThreads)
                            break;
                        Interlocked.Increment(ref downloaderRunning);
                        new Thread(() => ImageDownloader.Download(image)).Start();
                    }
                }
                Thread.Sleep(20);
            }
        }

        public static void Download(IImage image)
        {
            using (var client = new WebClient())
            {
                var url = image.LargeUrl;
                var fullPath = fileHandler.GetFullPath(image.Shortcode);
                try
                {
                    client.DownloadFile(new Uri(url), fullPath);
                    Console.WriteLine("successful downloaded: " + image.Shortcode);
                }
                catch (WebException e)
                {
                    if (e.Message.Contains("403"))
                    {
                        Console.WriteLine("Download failed with 403 at ID=" + image.Id);
                        // 403 is okay..
                    }
                    else if (e.Message.Contains("404"))
                    {
                        Console.WriteLine("Download failed with 404 at ID=" + image.Id);
                        // 403 is okay..
                    }
                    else
                    {
                        Console.WriteLine("Crashed at ID=" + image.Id);
                        Console.WriteLine(e.Message);
                    }
                    fileHandler.Delete(image.Shortcode);
                }
                finally
                {
                    Interlocked.Decrement(ref downloaderRunning);
                }
            }
        }

    }
}
