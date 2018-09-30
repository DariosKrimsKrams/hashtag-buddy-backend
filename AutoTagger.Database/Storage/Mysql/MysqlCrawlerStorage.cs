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

        public void InsertImage(IImage image)
        {
            if (image.HumanoidTags == null)
                return;

            var query = $"REPLACE INTO photos (`largeUrl`, `thumbUrl`, `shortcode`, `likes`, `comments`, `user`, `follower`, `following`, `posts`, `location_id`, `uploaded`) VALUES ('{image.LargeUrl}', '{image.ThumbUrl}', '{image.Shortcode}', '{image.Likes}', '{image.Comments}', '{image.User.Username}', '{image.User.FollowerCount}', '{image.User.FollowingCount}', '{image.User.PostCount}', '{image.Location}', '{image.Uploaded}')";
            var (_, time) = this.ExecuteCustomQuery(query);
            this.timingsImages.Add(time);

            // ToDo Batch

            this.InsertRelations(image);
        }

        private void InsertRelations(IImage image)
        {
            var values = "";
            foreach (var humanoidTag in image.HumanoidTags)
            {
                var id = this.GetHumanoidTagId(humanoidTag);
                values += $"('{image.Shortcode}', '{id}'),";
            }
            values = values.TrimEnd(',');
            var query = $"INSERT INTO photo_itag_rel (`shortcode`, `itagId`) VALUES {values};";
            var (_, time) = this.ExecuteCustomQuery(query);
            this.timingsRels.Add(time);
        }

        private int GetHumanoidTagId(string name)
        {
            var query  = $"SELECT `id` FROM itags WHERE `name`='{name}'";
            // ToDo remove id and use name as key!
            var (result, _) = this.ExecuteCustomQuery(query);
            var value  = result.FirstOrDefault()?.FirstOrDefault();
            if (value == null)
            {
                throw new InvalidOperationException("Itag doesn't exist. It must exist in DB");
            }
            return Convert.ToInt32(value);
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
                values += $"('{hTag.Name}', '{hTag.Posts}'),";
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
