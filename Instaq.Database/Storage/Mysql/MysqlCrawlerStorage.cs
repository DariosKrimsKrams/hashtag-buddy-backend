namespace Instaq.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Instaq.Contract.Models;

    using global::Instaq.Contract;

    using Instaq.Database.Storage.Mysql.Generated;

    public class MysqlCrawlerStorage : MysqlBaseStorage, ICrawlerStorage
    {
        private readonly List<TimeSpan> timingsImages = new List<TimeSpan>();
        private readonly List<TimeSpan> timingsRels = new List<TimeSpan>();
        private readonly List<TimeSpan> timingsHTags = new List<TimeSpan>();

        public MysqlCrawlerStorage(InstaqProdContext context)
            : base(context)
        {
        }

        public void InsertImages(IImage[] images)
        {
            var values = "";
            foreach (var image in images)
            {
                if (image?.HumanoidTags is null)
                {
                    continue;
                }

                var date = image.Uploaded;
                var uploaded = $"{date.Year}-{date.Month}-{date.Day} {date.Hour}:{date.Minute}:{date.Second}";
                values += $"('{image.LargeUrl}', '{image.ThumbUrl}', '{image.Shortcode}', '{image.Likes}', '{image.CommentCount}', '{image.User.Username}', '{image.User.FollowerCount}', '{image.User.FollowingCount}', '{image.User.PostCount}', '{uploaded}'),";
            }
            if (string.IsNullOrEmpty(values))
            {
                return;
            }
            values = values.TrimEnd(',');
            var query = $"INSERT INTO photos (`largeUrl`, `thumbUrl`, `shortcode`, `likes`, `comments`, `user`, `follower`, `following`, `posts`, `uploaded`) VALUES {values} "
                      + $"ON DUPLICATE KEY UPDATE `follower`=VALUES(`follower`), `following`=VALUES(`following`), `posts`=VALUES(`posts`), `likes`=VALUES(`likes`), `comments`=VALUES(`comments`);";

            var (_, time) = this.ExecuteCustomQuery(query);
            this.timingsImages.Add(time);

            this.InsertRelations(images);
        }

        private void InsertRelations(IImage[] images)
        {
            var values = "";
            foreach (var image in images)
            {
                foreach (var humanoidTag in image.HumanoidTags)
                {
                    values += $"('{image.Shortcode}', '{humanoidTag}'),";
                }
            }
            if (string.IsNullOrEmpty(values))
            {
                return;
            }
            values = values.TrimEnd(',');
            var query = $"INSERT IGNORE INTO photo_itag_rel (`shortcode`, `itag`) VALUES {values};";
            var (_, time) = this.ExecuteCustomQuery(query);
            this.timingsRels.Add(time);
        }

        public void InsertHumanoidTags(IHumanoidTag[] hTags)
        {
            if (hTags.Length == 0)
            {
                return;
            }

            var values = "";
            for (var i = 0; i < hTags.Length; i++)
            {
                var hTag = hTags[i];
                if (hTag is null)
                {
                    continue;
                }
                values += $"('{hTag.Name}', '{hTag.Posts}'),";
            }

            if (string.IsNullOrEmpty(values))
            {
                return;
            }
            values = values.TrimEnd(',');
            var query = $"INSERT INTO itags (`name`, `posts`) VALUES {values} ON DUPLICATE KEY UPDATE `posts`=VALUES(`posts`);";
            var (_, time) = this.ExecuteCustomQuery(query);
            this.timingsHTags.Add(time);
        }

        public IEnumerable<TimeSpan> GetTimings(string type)
        {
            IEnumerable<TimeSpan> output;
            switch (type)
            {
                case "images":
                    output = this.timingsImages.ToArray();
                    this.timingsImages.Clear();
                    return output;
                case "rels":
                    output = this.timingsRels.ToArray();
                    this.timingsRels.Clear();
                    return output;
                case "htags":
                    output = this.timingsHTags.ToArray();
                    this.timingsHTags.Clear();
                    return output;
            }

            return null;
        }
    }
}
