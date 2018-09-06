namespace AutoTagger.Crawler.V4.Crawler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4.PageAnalyzer;
    using AutoTagger.Crawler.V4.Requests;

    public class UserPageCrawler : BaseImagePageCrawler
    {
        private UserPageHandler handler;

        public UserPageCrawler(ICrawlerSettings settings, IRequestHandler requestHandler)
        {
            this.handler = new UserPageHandler(settings);

            this.Settings = settings;
            this.RequestHandler = requestHandler;
            this.MinCommentsCount = this.Settings.UserMinHashTagCount;
            this.MinHashTagCount  = this.Settings.UserMinCommentsCount;
            this.MinLikes         = this.Settings.UserMinLikes;
        }

        public IUser Parse(string url)
        {
            var data = this.GetData(url);

            var user = this.GetUser(data);

            if (!this.handler.HasUserEnoughFollower(user))
            {
                return user;
            }

            var nodes = GetTimelineMediaNodes(data);
            user.Images = this.GetImages(nodes);
            user.Images = this.handler.RemoveImagesWithDuplicateHashtags(user.Images);

            return user;
        }

        private User GetUser(dynamic data)
        {
            var user = new User();
            var node = data?.entry_data?.ProfilePage?[0]?.graphql?.user;
            user.FollowerCount = Convert.ToInt32(node?.edge_followed_by?.count.ToString());
            user.FollowingCount = Convert.ToInt32(node?.edge_follow?.count.ToString());
            user.PostCount = Convert.ToInt32(node?.edge_owner_to_timeline_media?.count.ToString());
            user.Username = node?.username;
            return user;
        }

        private dynamic GetTimelineMediaNodes(dynamic data)
        {
            return data?.entry_data?.ProfilePage?[0]?.graphql?.user?.edge_owner_to_timeline_media?.edges;
        }
    }
}
