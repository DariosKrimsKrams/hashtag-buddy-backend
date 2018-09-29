namespace AutoTagger.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Database.Storage.Mysql.Generated;

    using global::AutoTagger.Contract;

    using Instaq.Logger;

    public class MysqlCrawlerStorage : MysqlBaseStorage, ICrawlerStorage
    {
        private List<Itags> allITags;

        public void Upsert(IImage image)
        {
            try
            {
                var existingPhoto = this.GetExistingPhoto(image);
                existingPhoto.Likes     = image.Likes;
                existingPhoto.Comments  = image.CommentCount;
                existingPhoto.Follower = image.User.FollowerCount;
                existingPhoto.Following = image.User.FollowingCount;
                existingPhoto.Posts     = image.User.PostCount;
            }
            catch
            {
                this.Insert(image);
            }
        }

        private Photos GetExistingPhoto(IImage image)
        {
            var query = $"SELECT `id`, `likes`, `comments`, `follower`, `following`, `posts` FROM photos WHERE `shortcode` = '{image.Shortcode}' LIMIT 1";
            var output = this.ExecutePhotosQuery(query);
            var entry = output.FirstOrDefault();
            if (entry == null)
            {
                throw new Exception();
            }
            return entry;
        }

        private void Insert(IImage image)
        {
            if (image.HumanoidTags == null)
                return;

            var query = $"INSERT INTO photos (`largeUrl`, `thumbUrl`, `shortcode`, `likes`, `comments`, `user`, `follower`, `following`, `posts`, `location_id`, `uploaded`) VALUES('{image.LargeUrl}', '{image.ThumbUrl}', '{image.Shortcode}', '{image.Likes}', '{image.Comments}', '{image.User.Username}', '{image.User.FollowerCount}', '{image.User.FollowingCount}', '{image.User.PostCount}', '{image.Location}', '{image.Uploaded}'); SELECT LAST_INSERT_ID();";
            var output = this.ExecuteCustomQuery(query);
            var photoId = Convert.ToInt32(output.FirstOrDefault()?.FirstOrDefault());

            this.InsertRelations(image.HumanoidTags, photoId);
        }

        private void InsertRelations(IEnumerable<string> humanoidTags, int photoId)
        {
            foreach (var iTagName in humanoidTags)
            {
                var itag = this.allITags.SingleOrDefault(x => x.Name == iTagName);
                if (itag == null)
                {
                    throw new InvalidOperationException("ITag must exist in DB");
                }

                var query = $"INSERT INTO photo_itag_rel (`photoId`, `itagId`) VALUES ('{photoId}', '{itag.Id}');";
                this.ExecuteCustomQuery(query);
            }
        }

        public void FullHumanoidTags()
        {
            this.allITags = this.db.Itags.ToList();
            //this.allITags.ForEach(
            //    x =>
            //    {
            //        Logging.LogInline(x.Name + ", ");
            //    });
            //Logging.Log("_________________");
        }

        public IEnumerable<IHumanoidTag> GetAllHumanoidTags<T>() where T : IHumanoidTag
        {
            this.FullHumanoidTags();
            var hTags = new List<IHumanoidTag>();
            foreach (var iTag in this.allITags)
            {
                var t = (T)Activator.CreateInstance(typeof(T));
                t.Name = iTag.Name;
                t.Posts = iTag.Posts;
                hTags.Add(t);
            }
            return hTags;
        }

        public void UpsertHumanoidTag(IHumanoidTag hTag)
        {
            var existingHumanoidTag = this.allITags.FirstOrDefault(x => x.Name == hTag.Name);
            if (existingHumanoidTag != null)
            {
                if (hTag.Posts == 0)
                    return;
                Logging.Log("Update " + hTag.Name);
                existingHumanoidTag.Posts = hTag.Posts;
                this.UpdateHumanoidTag(hTag);
            }
            else
            {
                Logging.Log("Insert " + hTag.Name);
                this.InsertHumanoidTag(hTag);
            }
        }

        private void UpdateHumanoidTag(IHumanoidTag hTag)
        {
            var query = $"UPDATE itags SET Posts = '{hTag.Posts}' WHERE id = '{hTag.Id}'";
            this.ExecuteCustomQuery(query);
        }

        private void InsertHumanoidTag(IHumanoidTag hTag)
        {
            var query = $"INSERT INTO itags (`Name`, `Posts`) VALUES ('{hTag.Name}', '{hTag.Posts}'); SELECT LAST_INSERT_ID();";
            var output = this.ExecuteCustomQuery(query);
            var id = Convert.ToInt32(output.FirstOrDefault()?.FirstOrDefault());
            var itag = new Itags { Id = id, Name = hTag.Name, Posts = hTag.Posts };
            this.allITags.Add(itag);
        }

    }
}
