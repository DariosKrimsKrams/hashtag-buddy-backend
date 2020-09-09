namespace Instaq.ImageDownloader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using Instaq.Contract;
    using Instaq.Contract.Models;
    using Instaq.Contract.Storage;
    using Instaq.DiskFileHandling;

    public class ImageDownloader
    {
        private static IImageProcessorStorage storage;
        private static int threadsRunning;
        private const int QueryImagesAtLessOrEqualImages = 30;
        private const int DbGetLimit = 300;
        private const int ParallelThreads = 100;
        private static IFileHandler fileHandler;
        private static List<string> files;
        private List<string> downloadedFiles;
        private static int logCount = 0;
        private readonly List<string> imagesToSetDownloadedStatus;
        private readonly List<string> imagesToSetFailedStatus;
        private readonly List<string> imagesToSet404Status;

        private enum StorageUses
        {
            None,
            Get,
            Update
        }

        private static StorageUses CurStorageUse;

        public ImageDownloader(IImageProcessorStorage db)
        {
            storage = db;
            fileHandler = new DiskFileHander();
            this.imagesToSetDownloadedStatus = new List<string>();
            this.imagesToSetFailedStatus = new List<string>();
            this.imagesToSet404Status = new List<string>();
            this.downloadedFiles = new List<string>();
        }

        public void Start()
        {
            files = fileHandler.GetAllUnusedImages().ToList();
            new Thread(this.GetImages).Start();
            new Thread(Logs).Start();
            new Thread(this.DbUpdater).Start();
        }

        private void DbUpdater()
        {
            while (true)
            {
                this.DbUpdaterLogic(this.imagesToSetDownloadedStatus, "downloaded");
                this.DbUpdaterLogic(this.imagesToSetFailedStatus, "failed");
                this.DbUpdaterLogic(this.imagesToSet404Status, "404");
                Thread.Sleep(500);
            }
        }

        private void DbUpdaterLogic(IList<string> input, string status)
        {
            var count = input.Count;
            if (count == 0 || !this.TrySetStatus(StorageUses.Update))
            {
                return;
            }
            storage.SetImagesStatus(input.ToList(), status);
            for (var i = count - 1; i >= 0; i--)
            {
                input.RemoveAt(i);
            }
            Console.WriteLine($"Update {count} Photos to {status}");
            CurStorageUse = StorageUses.None;
        }

        private bool TrySetStatus(StorageUses newStatus)
        {
            if (CurStorageUse == newStatus)
            {
                return true;
            }
            if (CurStorageUse != StorageUses.None)
            {
                return false;
            }
            CurStorageUse = newStatus;
            return CurStorageUse == newStatus;
        }

        public void GetImages()
        {
            var delay = 30;
            while (true)
            {
                if (threadsRunning <= QueryImagesAtLessOrEqualImages)
                {
                    delay = 500;
                    if (this.TrySetStatus(StorageUses.Get))
                    {
                        Console.WriteLine("Start Getting Photos");
                        var images = storage.GetImagesWithEmptyStatus(DbGetLimit + this.downloadedFiles.Count);
                        CurStorageUse = StorageUses.None;
                        var enumerable = images as IImage[] ?? images.ToArray();
                        Console.WriteLine("Get " + enumerable.Count() + " DB Entries");
                        foreach (var image in enumerable)
                        {
                            this.DownloaderHandling(image);
                            delay = 30;
                        }
                    }
                }
                Thread.Sleep(delay);
            }
        }

        private void DownloaderHandling(IImage image)
        {
            if (threadsRunning >= ParallelThreads)
            {
                Thread.Sleep(30);
                this.DownloaderHandling(image);
                return;
            }
            if (this.downloadedFiles.Contains(image.Shortcode))
            {
                return;
            }
            if (files.Contains(image.Shortcode))
            {
                // if it was on disk before, but not flaged in storage
                //  -> remove on disk
                fileHandler.Delete(image.Shortcode);
            }

            this.downloadedFiles.Add(image.Shortcode);
            Interlocked.Increment(ref threadsRunning);
            new Thread(() => this.Download(image)).Start();
        }

        public void Download(IImage image)
        {
            using (var client = new WebClient())
            {
                var url = image.LargeUrl;
                var fullPath = fileHandler.GetFullPath(image.Shortcode);
                try
                {
                    client.DownloadFile(new Uri(url), fullPath);
                    Console.WriteLine("successful downloaded: " + image.Shortcode + " (" + image.Created + ")");
                    this.imagesToSetDownloadedStatus.Add(image.Shortcode);
                    logCount++;
                }
                catch (WebException e)
                {
                    if (e.Message.Contains("403"))
                    {
                        Console.WriteLine("Download failed with 403 at Created=" + image.Created);
                        this.imagesToSetFailedStatus.Add(image.Shortcode);
                    }
                    else if (e.Message.Contains("404"))
                    {
                        Console.WriteLine("Download failed with 404 at Created=" + image.Created);
                        this.imagesToSet404Status.Add(image.Shortcode);
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
                    Interlocked.Decrement(ref threadsRunning);
                }
            }
        }

        private static void Logs()
        {
            while (true)
            {
                if (logCount == 0)
                {
                    continue;
                }
                Console.WriteLine("Downloaded since start: " + logCount);
                Thread.Sleep(5000);
            }
        }

    }
}
