namespace AutoTagger.ImageDownloader.Standard
{
    using System;
    using System.Collections.Concurrent;
    using System.Net;
    using System.Threading;
    using AutoTagger.Contract;

    public class ImageDownloader
    {
        private static IImageProcessorStorage storage;
        private static int downloaderRunning;

        private static readonly int QueryImagesAtLessOrEqualImages = 20;
        private static readonly int DbSelectImagesAmount = 100;
        private static readonly string Path = @"C:\Instagger\";

        public ImageDownloader(IImageProcessorStorage db)
        {
            storage = db;
            System.IO.Directory.CreateDirectory(Path);
        }

        public void Start()
        {
            new Thread(ImageDownloader.GetImages).Start();
        }

        public static void GetImages()
        {
            var lastId = 78887;
            while (true)
            {
                if (downloaderRunning <= QueryImagesAtLessOrEqualImages)
                {
                    Console.WriteLine("DB Get Images");
                    var images = storage.GetImagesWithoutMachineTags(lastId, DbSelectImagesAmount);
                    foreach (var image in images)
                    {
                        Interlocked.Increment(ref downloaderRunning);
                        new Thread(() => ImageDownloader.Download(image)).Start();
                        lastId = image.Id;
                    }
                }
                Thread.Sleep(30);
            }
        }

        public static void Download(IImage image)
        {
            using (var client = new WebClient())
            {
                var url = image.LargeUrl;
                var filename = Path + image.Shortcode + ".jpg";
                try
                {
                    client.DownloadFile(new Uri(url), filename);
                    Console.WriteLine("successful downloaded: " + filename);
                }
                catch (WebException e)
                {
                    if (e.Message.Contains("403"))
                    {
                        Console.WriteLine("Download failed with 403 on ID=" + image.Id);
                        // 403 is okay..
                    }
                    else if (e.Message.Contains("404"))
                    {
                        Console.WriteLine("Download failed with 404 on ID=" + image.Id);
                        // 403 is okay..
                    }
                    else
                    {
                        throw;
                    }
                    System.IO.File.Delete(filename);
                }
                finally
                {
                    Interlocked.Decrement(ref downloaderRunning);
                }
            }
        }

    }
}
