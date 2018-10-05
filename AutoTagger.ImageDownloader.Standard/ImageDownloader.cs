namespace AutoTagger.ImageDownloader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;

    using AutoTagger.Contract;
    using AutoTagger.FileHandling.Standard;

    public class ImageDownloader
    {
        private static IImageProcessorStorage storage;
        private static int downloaderRunning;
        private const int QueryImagesAtLessOrEqualImages = 20;
        private const int DbSelectImagesAmount = 500;
        private const int ParallelThreads = 100;
        private static DateTime lastDate;
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
            lastDate = storage.GetCreatedDateForLatestPhotoWithMTags();
            new Thread(ImageDownloader.GetImages).Start();
        }

        public static void GetImages()
        {
            var delay = 30;
            while (true)
            {
                if (downloaderRunning <= QueryImagesAtLessOrEqualImages)
                {
                    delay = 500;
                    Console.WriteLine("Check DB for Created=" + lastDate);
                    var images = storage.GetImagesWithoutMachineTags(lastDate, DbSelectImagesAmount);
                    foreach (var image in images)
                    {
                        delay = 30;
                        lastDate = image.Created;
                        if (files.Contains(image.Shortcode))
                        {
                            continue;
                        }
                        if (downloaderRunning >= ParallelThreads)
                        {
                            break;
                        }
                        Interlocked.Increment(ref downloaderRunning);
                        new Thread(() => ImageDownloader.Download(image)).Start();
                    }
                }
                Thread.Sleep(delay);
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
                        Console.WriteLine("Download failed with 403 at Created=" + image.Created);
                    }
                    else if (e.Message.Contains("404"))
                    {
                        Console.WriteLine("Download failed with 404 at Created=" + image.Created);
                    }
                    else
                    {
                        Console.WriteLine("Crashed at Created=" + image.Created);
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
