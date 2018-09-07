namespace AutoTagger.Crawler.V4.Crawler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Crawler.V4.PageAnalyzer;
    using AutoTagger.Crawler.V4.Requests;

    public class UserPageCrawler
    {
        private readonly UserPageLogic userPageLogic;
        private readonly ImagePageLogic imagePageLogic;
        private ICrawlerSettings Settings;

        public UserPageCrawler(ICrawlerSettings settings,
                               IRequestHandler requestHandler)
        {
            this.Settings = settings;
            this.userPageLogic = new UserPageLogic(settings);
            this.imagePageLogic = new ImagePageLogic(settings, requestHandler);

            this.imagePageLogic.MinCommentsCount = this.Settings.UserMinHashTagCount;
            this.imagePageLogic.MinHashTagCount  = this.Settings.UserMinCommentsCount;
            this.imagePageLogic.MinLikes = this.Settings.UserMinLikes;
        }

        public IUser Parse(string url)
        {
            var data = this.imagePageLogic.GetData(url);

            var user = this.GetUser(data);

            if (!this.userPageLogic.HasUserEnoughFollower(user))
            {
                return user;
            }

            var nodes = GetTimelineMediaNodes(data);
            user.Images = this.imagePageLogic.GetImages(nodes);
            user.Images = this.imagePageLogic.RemoveUnrelevantImages(user.Images);
            user.Images = this.userPageLogic.RemoveImagesWithDuplicateHashtags(user.Images);

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
