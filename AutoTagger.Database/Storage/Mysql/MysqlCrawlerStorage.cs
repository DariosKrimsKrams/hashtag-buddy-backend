namespace AutoTagger.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::AutoTagger.Contract;

    public class MysqlCrawlerStorage : MysqlBaseStorage, ICrawlerStorage
    {
        private readonly List<TimeSpan> timingsImages = new List<TimeSpan>();
        private readonly List<TimeSpan> timingsRels = new List<TimeSpan>();
        private readonly List<TimeSpan> timingsHTags = new List<TimeSpan>();

        public void InsertImages(IImage[] images)
        {
            var values = "";
            foreach (var image in images)
            {
                if (image?.HumanoidTags == null)
                {
                    continue;
                }
                values += $"('{image.LargeUrl}', '{image.ThumbUrl}', '{image.Shortcode}', '{image.Likes}', '{image.CommentCount}', '{image.User.Username}', '{image.User.FollowerCount}', '{image.User.FollowingCount}', '{image.User.PostCount}', '{image.Uploaded}'),";
            }
            values = values.TrimEnd(',');
            var query = $"REPLACE INTO photos (`largeUrl`, `thumbUrl`, `shortcode`, `likes`, `comments`, `user`, `follower`, `following`, `posts`, `uploaded`) VALUES {values};";

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
                if (hTag == null)
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
            var query = $"REPLACE INTO itags (`Name`, `Posts`) VALUES {values};";
            var (_, time) = this.ExecuteCustomQuery(query);
            this.timingsHTags.Add(time);
        }

        public List<TimeSpan> GetTimings(string type)
        {
            List<TimeSpan> output;
            switch (type)
            {
                case "images":
                    output = this.timingsImages.ToList();
                    this.timingsImages.Clear();
                    return output;
                case "rels":
                    output = this.timingsRels.ToList();
                    this.timingsRels.Clear();
                    return output;
                case "htags":
                    output = this.timingsHTags.ToList();
                    this.timingsHTags.Clear();
                    return output;
            }

            return null;
        }
    }
}
