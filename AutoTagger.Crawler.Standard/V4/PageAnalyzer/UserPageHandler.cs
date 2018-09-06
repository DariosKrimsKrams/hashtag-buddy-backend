namespace AutoTagger.Crawler.V4.PageAnalyzer
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Common;
    using AutoTagger.Contract;

    public class UserPageHandler
    {
        protected ICrawlerSettings Settings;

        public UserPageHandler(ICrawlerSettings settings)
        {
            this.Settings = settings;
        }

        public bool HasUserEnoughFollower(IUser user)
        {
            return user.FollowerCount >= this.Settings.UserMinFollowerCount;
        }

        public IEnumerable<IImage> RemoveImagesWithDuplicateHashtags(IList<IImage> images)
        {
            var newImages = new Dictionary<string, IImage>();
            for (var i = images.Count - 1; i >= 0; i--)
            {
                var image    = images[i];
                var hashTags = string.Join("", image.HumanoidTags);
                if (!newImages.ContainsKey(hashTags))
                    newImages.Add(hashTags, image);
            }
            return newImages.Select(x => x.Value);
        }
    }
}
