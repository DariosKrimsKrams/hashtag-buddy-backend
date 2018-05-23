﻿namespace AutoTagger.ImageProcessor.Standard
{
    using System;
    using System.Collections.Generic;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.Standard;
    using Google.Cloud.Vision.V1;
    using Image = Google.Cloud.Vision.V1.Image;

    public class GCPVision : ITaggingProvider
    {
        private const string keyLabel = "GCPVision_Label";
        private const string keyWeb = "GCPVision_Web";
        private readonly ImageAnnotatorClient client;

        public GCPVision()
        {
            this.client = ImageAnnotatorClient.Create();
        }

        public IEnumerable<IMTag> GetTagsForFile(string filename)
        {
            Image image = null;
            try
            {
                image = Image.FromFile(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (e.Message.Contains("Could not find file"))
                {
                    yield break;
                }
            }

            var mTags = this.Detect(image);
            foreach (var mTag in mTags)
            {
                yield return mTag;
            }
        }

        public IEnumerable<IMTag> GetTagsForImageBytes(byte[] bytes)
        {
            var image = Image.FromBytes(bytes);

            var mTags = this.Detect(image);
            foreach (var mTag in mTags)
            {
                yield return mTag;
            }
        }

        public IEnumerable<IMTag> GetTagsForImageUrl(string imageUrl)
        {
            var image = Image.FromUri(imageUrl);

            var mTags = this.Detect(image);
            foreach (var mTag in mTags)
            {
                yield return mTag;
            }
        }

        private IEnumerable<IMTag> Detect(Image image)
        {
            IReadOnlyList<EntityAnnotation> labels = null;
            WebDetection webInfos = null;

            try
            {
                labels   = this.client.DetectLabels(image);
                webInfos = this.client.DetectWebInformation(image);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("The URL does not appear to be accessible by us.")
                 || e.Message.Contains("We can not access the URL currently."))
                {
                    yield break;
                }
                if (!e.Message.Contains("Bad image data"))
                {
                    Console.WriteLine(e);
                }
                throw;
            }

            if (labels == null || webInfos == null)
                yield break;

            foreach (var mTag in ToMTags(labels, webInfos))
            {
                yield return mTag;
            }
        }

        private static IEnumerable<IMTag> ToMTags(IReadOnlyList<EntityAnnotation> labels, WebDetection webInfos)
        {
            foreach (var x in labels)
            {
                if (String.IsNullOrEmpty(x.Description))
                    continue;
                var mtag = new MTag { Name = x.Description, Score = x.Score, Source = keyLabel };
                yield return mtag;
            }

            foreach (var x in webInfos.WebEntities)
            {
                if (String.IsNullOrEmpty(x.Description))
                    continue;
                var mtag = new MTag { Name = x.Description, Score = x.Score, Source = keyWeb };
                yield return mtag;
            }
        }
    }
}
