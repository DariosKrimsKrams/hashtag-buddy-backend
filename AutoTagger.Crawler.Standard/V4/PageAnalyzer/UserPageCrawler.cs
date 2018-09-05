namespace AutoTagger.Crawler.V4.Crawler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4.Requests;

    public class UserPageCrawler : BaseImagePageCrawler
    {
        private readonly int MinFollowerCount;

        public UserPageCrawler(CrawlerSettings settings, IRequestHandler requestHandler)
        {
            this.Settings         = settings;
            this.requestHandler   = requestHandler;
            this.MinFollowerCount = this.Settings.UserMinFollowerCount;
            this.MinCommentsCount = this.Settings.UserMinHashTagCount;
            this.MinHashTagCount  = this.Settings.UserMinCommentsCount;
            this.MinLikes         = this.Settings.UserMinLikes;
        }

        public IEnumerable<IImage> Parse(string url)
        {
            var data = this.GetData(url);

            if (!HasUserEnoughFollower(data, out int followerCount, out int followingCount, out int postsCount))
            {
                return new List<IImage>();
            }

            var nodes = GetNodes(data);
            var images = this.GetImages(nodes);

            foreach (IImage image in images)
            {
                image.Follower  = followerCount;
                image.Following = followingCount;
                image.Posts = postsCount;
            }

            return this.RemoveImagesWithDuplicateHashtags(images);
        }

        private IEnumerable<IImage> RemoveImagesWithDuplicateHashtags(IList<IImage> images)
        {
            var newImages = new Dictionary<string, IImage>();
            for (var i = images.Count-1; i >= 0; i--)
            {
                var image = images[i];
                var hashTags = string.Join("", image.HumanoidTags);
                if (!newImages.ContainsKey(hashTags))
                    newImages.Add(hashTags, image);
            }
            return newImages.Select(x => x.Value);
        }

        private bool HasUserEnoughFollower(dynamic data, out int followerCount, out int followingCount, out int postsCount)
        {
            var node      = data?.entry_data?.ProfilePage?[0]?.graphql?.user;
            followerCount = Convert.ToInt32(node?.edge_followed_by?.count.ToString());
            followingCount = Convert.ToInt32(node?.edge_follow?.count.ToString());
            postsCount = Convert.ToInt32(node?.edge_owner_to_timeline_media?.count.ToString());
            return followerCount >= MinFollowerCount;
        }

        private dynamic GetNodes(dynamic data)
        {
            return data?.entry_data?.ProfilePage?[0]?.graphql?.user?.edge_owner_to_timeline_media?.edges;
        }
    }
}
