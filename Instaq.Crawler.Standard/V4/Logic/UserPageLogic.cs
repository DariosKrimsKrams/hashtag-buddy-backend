namespace AutoTagger.Crawler.V4.PageAnalyzer
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

    public class UserPageLogic
    {
        protected ICrawlerSettings Settings;

        public UserPageLogic(ICrawlerSettings settings)
        {
            this.Settings = settings;
        }

        public bool HasUserEnoughFollower(IUser user)
        {
            return user.FollowerCount >= this.Settings.UserMinFollowerCount;
        }

        public IEnumerable<IImage> RemoveImagesWithIdenticalHashtags(IEnumerable<IImage> images)
        {
            var usedHashtags = new List<string>();
            return images.Reverse().Select(
                x =>
                {
                    var hashtags = string.Join("", x.HumanoidTags);
                    if (!usedHashtags.Contains(hashtags))
                    {
                        usedHashtags.Add(hashtags);
                        return x;
                    }
                    return null;
                }).Where(x => x != null).Reverse().ToList();
        }
    }
}
