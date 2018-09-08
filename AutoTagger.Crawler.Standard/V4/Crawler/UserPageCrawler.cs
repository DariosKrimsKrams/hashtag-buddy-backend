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
        private readonly ICrawlerSettings settings;

        public UserPageCrawler(ICrawlerSettings settings,
                               IRequestHandler requestHandler)
        {
            this.settings = settings;
            this.userPageLogic = new UserPageLogic(settings);
            this.imagePageLogic = new ImagePageLogic(settings, requestHandler);

            this.imagePageLogic.MinCommentsCount = this.settings.UserMinHashTagCount;
            this.imagePageLogic.MinHashTagCount  = this.settings.UserMinCommentsCount;
            this.imagePageLogic.MinLikes = this.settings.UserMinLikes;
        }

        public IUser Parse(string url)
        {
            var nodes = this.imagePageLogic.GetData(url);

            var user = this.GetUser(nodes);

            if (!this.userPageLogic.HasUserEnoughFollower(user))
            {
                user.Images = Enumerable.Empty<IImage>();
                return user;
            }

            var timelineMediaNodes = GetTimelineMediaNodes(nodes);
            user.Images = this.imagePageLogic.GetImages(timelineMediaNodes);
            user.Images = this.imagePageLogic.RemoveUnrelevantImages(user.Images);
            user.Images = this.userPageLogic.RemoveImagesWithIdenticalHashtags(user.Images);

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
