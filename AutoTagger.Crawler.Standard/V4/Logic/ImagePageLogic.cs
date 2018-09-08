namespace AutoTagger.Crawler.V4.Crawler
{
    using System;
    using System.Collections;
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
                var images = this.GetImage(innerNode);
                foreach (var image in images)
                {
                    yield return image;
                }
            }
        }

        public IEnumerable<IImage> GetImage(dynamic node)
        {
            var edges = node?.edge_media_to_caption?.edges;
            if (edges == null || edges.ToString() == "[]")
            {
                yield break;
            }

            string message = edges[0]?.node?.text;
            var hashtags = this.ParseHashtags(message);
            var comments = GetComments(node);
            GetHashtagsFromComments(comments, ref hashtags);
            var thumb = node?.thumbnail_src ?? node?.display_resources?[0].src;

            var takenDate = GetDateTime(Convert.ToDouble(node?.taken_at_timestamp));
            var image = new Image
            {
                Likes        = node.edge_media_preview_like?.count,
                CommentCount = node?.edge_media_to_comment?.count,
                Message      = message,
                Comments     = comments,
                Shortcode    = node?.shortcode,
                HumanoidTags = hashtags,
                LargeUrl     = node?.display_url,
                ThumbUrl     = thumb,
                Uploaded     = takenDate,
                Location     = GetLocation(node),
            };

            yield return image;
        }

        private Location GetLocation(dynamic node)
        {
            var locationNode = node?.location;
            if (locationNode != null)
            {
                return new Location
                {
                    Id            = locationNode.id,
                    HasPublicPage = locationNode.has_public_page,
                    Name          = locationNode.name,
                    Slug          = locationNode.slug
                };
            }
            return null;
        }

        private IEnumerable<string> GetComments(dynamic node)
        {
            var commentsNode = node?.edge_media_to_comment?.edges;
            if (commentsNode == null)
            {
                yield break;
            }
            foreach (var commentNode in commentsNode)
            {
                var commentText = commentNode?.node?.text.ToString();
                yield return commentText;
            }
        }


        private void GetHashtagsFromComments(IEnumerable<string> comments, ref IEnumerable<string> hashtags)
        {
            foreach (var comment in comments)
            {
                var commentHashtags = this.ParseHashtags(comment);
                hashtags = hashtags.Concat(commentHashtags);
            }
            hashtags = hashtags.Distinct();
        }

        public IEnumerable<string> ParseHashtags(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Enumerable.Empty<string>();
            }
            text = text.Replace("\\n", "\n");
            text = System.Web.HttpUtility.HtmlDecode(text);
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
