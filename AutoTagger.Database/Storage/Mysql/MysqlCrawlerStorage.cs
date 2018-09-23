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
            var photo = Photos.FromImage(image);

            try
            {
                var existingPhoto = this.GetExistingPhoto(photo);
                existingPhoto.Likes     = photo.Likes;
                existingPhoto.Comments  = photo.Comments;
                existingPhoto.Following = photo.Following;
                existingPhoto.Posts     = photo.Posts;
            }
            catch
            {
                this.Insert(image, photo);
            }
            //this.Save();
        }

        private Photos GetExistingPhoto(Photos photo)
        {
            return this.db.Photos.First(x => x.Shortcode == photo.Shortcode);
        }

        private void Insert(IImage image, Photos photo)
        {
            if (image.HumanoidTags == null)
                return;

            foreach (var iTagName in image.HumanoidTags)
            {
                var itag = this.allITags.SingleOrDefault(x => x.Name == iTagName);
                if (itag == null)
                {
                    throw new InvalidOperationException("ITag must exist in DB");
                }

                var rel = new PhotoItagRel { Itag = itag, Photo = photo };
                photo.PhotoItagRel.Add(rel);
            }

            this.db.Photos.Add(photo);
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
            var query = $"INSERT INTO itags (Name, Posts) VALUES ('{hTag.Name}', '{hTag.Posts}'); SELECT LAST_INSERT_ID();";
            var output = this.ExecuteCustomQuery(query);
            //var id = Convert.ToInt32(output.FirstOrDefault()?.FirstOrDefault());
            //var itag = new Itags { Id = id, Name = hTag.Name, Posts = hTag.Posts };
            var itag = this.db.Itags.OrderByDescending(x => x.Id).Take(1).FirstOrDefault();
            this.allITags.Add(itag);
        }

        public new void Save()
        {
            Logging.Log("________________ Save ________________");
            base.Save();
        }

    }
}
