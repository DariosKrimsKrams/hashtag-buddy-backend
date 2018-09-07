namespace AutoTagger.Crawler.V4.Crawler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4.Requests;

    public class ImagePageLogic
    {
        private static readonly Regex FindHashTagsRegex = new Regex(@"#\w+", RegexOptions.Compiled);

        private readonly ICrawlerSettings settings;
        private readonly IRequestHandler requestHandler;

        public int MinCommentsCount;
        public int MinHashTagCount;
        public int MinLikes;

        public ImagePageLogic(ICrawlerSettings settings,
                                IRequestHandler requestHandler)
        {
            this.settings = settings;
            this.requestHandler = requestHandler;
        }

        private static DateTime GetDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        }

        public dynamic GetData(string url)
        {
            return this.requestHandler.FetchNode(url);
        }

        public IEnumerable<IImage> GetImages(dynamic nodes)
        {
            if (nodes == null)
            {
                yield break;
            }

            foreach (var node in nodes)
            {
                var innerNode = node?.node;
                var edges = innerNode?.edge_media_to_caption?.edges;
                if (edges == null || edges.ToString() == "[]")
                {
                    continue;
                }

                string text = edges[0]?.node?.text;
                text = text?.Replace("\\n", "\n");
                text = System.Web.HttpUtility.HtmlDecode(text);
                var hashtags = this.ParseHashtags(text).ToList();

                var comments = innerNode?.edge_media_to_comment?.edges;

                var takenDate = GetDateTime(Convert.ToDouble(innerNode?.taken_at_timestamp.ToString()));
                var image = new Image
                {
                    Likes = innerNode.edge_liked_by?.count,
                    CommentCount = innerNode?.edge_media_to_comment?.count,
                    Comments = comments,
                    Shortcode = innerNode?.shortcode,
                    HumanoidTags = hashtags,
                    LargeUrl = innerNode?.display_url,
                    ThumbUrl = innerNode?.thumbnail_src,
                    Uploaded = takenDate
                };

                var locationNode = innerNode?.location;
                if (locationNode != null)
                {
                    var location = new Location
                    {
                        Id            = locationNode.id,
                        HasPublicPage = locationNode.has_public_page,
                        Name          = locationNode.name,
                        Slug          = locationNode.slug
                    };
                    image.Location = location;
                }

                yield return image;
            }
        }

        public IEnumerable<string> ParseHashtags(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Enumerable.Empty<string>();
            }
            return FindHashTagsRegex
                .Matches(text)
                .OfType<Match>()
                .Select(m => m?.Value
                    .Trim(' ', '#')
                    .ToLower())
                .Where(this.HashtagIsAllowed).Distinct();
        }

        public bool HashtagIsAllowed(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                && value != ""
                && value.Length >= this.settings.MinHashtagLength
                && (value.Length <= this.settings.MaxHashtagLength || this.settings.MaxHashtagLength == 0)
                && !IsDigitsOnly(value)
                && !value.Contains('\'');
        }

        private static bool IsDigitsOnly(string str)
        {
            foreach (var c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        public IEnumerable<IImage> RemoveUnrelevantImages(IEnumerable<IImage> images)
        {
            foreach (var image in images)
            {
                if (this.ImageMeetsCriteria(image))
                {
                    yield return image;
                }
            }
        }

        public bool ImageMeetsCriteria(IImage image)
        {
            return image.HumanoidTags.Count() >= this.MinHashTagCount
                && image.Likes >= this.MinLikes
                && image.CommentCount >= this.MinCommentsCount;
        }

    }
}
