namespace AutoTagger.Crawler.V4.PageAnalyzer
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Common;
    using AutoTagger.Contract;

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

        public IEnumerable<IImage> RemoveImagesWithDuplicateHashtags(IEnumerable<IImage> images)
        {
            var imagesList = images.ToList();
            var newImages = new Dictionary<string, IImage>();
            for (var i = imagesList.Count() - 1; i >= 0; i--)
            {
                var image    = imagesList[i];
                var hashTags = string.Join("", image.HumanoidTags);
                if (!newImages.ContainsKey(hashTags))
                    newImages.Add(hashTags, image);
            }
            return newImages.Select(x => x.Value);
        }
    }
}
