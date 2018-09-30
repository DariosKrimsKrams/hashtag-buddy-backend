namespace AutoTagger.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using AutoTagger.Database.Storage.Mysql.Generated;

    using global::AutoTagger.Contract;

    using Instaq.Logger;

    public class MysqlCrawlerStorage : MysqlBaseStorage, ICrawlerStorage
    {
        public void InsertImage(IImage image)
        {
            if (image.HumanoidTags == null)
                return;

            var query = $"REPLACE INTO photos (`largeUrl`, `thumbUrl`, `shortcode`, `likes`, `comments`, `user`, `follower`, `following`, `posts`, `location_id`, `uploaded`) VALUES ('{image.LargeUrl}', '{image.ThumbUrl}', '{image.Shortcode}', '{image.Likes}', '{image.Comments}', '{image.User.Username}', '{image.User.FollowerCount}', '{image.User.FollowingCount}', '{image.User.PostCount}', '{image.Location}', '{image.Uploaded}'); SELECT LAST_INSERT_ID();";
            var output = this.ExecuteCustomQuery(query);
            var photoId = Convert.ToInt32(output.FirstOrDefault()?.FirstOrDefault());

            this.InsertRelations(image.HumanoidTags, photoId);
        }

        private void InsertRelations(IEnumerable<string> humanoidTags, int photoId)
        {
            var values = "";
            foreach (var humanoidTag in humanoidTags)
            {
                var id = this.GetHumanoidTagId(humanoidTag);
                values += $"('{photoId}', '{id}'),";
            }
            values = values.TrimEnd(',');
            var query = $"INSERT INTO photo_itag_rel (`photoId`, `itagId`) VALUES {values};";
            this.ExecuteCustomQuery(query);
        }

        private int GetHumanoidTagId(string name)
        {
            var query  = $"SELECT `id` FROM itags WHERE `name`='{name}'";
            var result = this.ExecuteCustomQuery(query);
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
            this.ExecuteCustomQuery(query);
        }

    }
}
