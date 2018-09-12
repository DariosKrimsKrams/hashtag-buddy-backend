namespace AutoTagger.Database.Storage.Mysql
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoTagger.Database.Storage.Mysql.Generated;

    using global::AutoTagger.Contract;

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
            this.Save();
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
                    throw new InvalidOperationException("ITag must exists in DB");
                }

                var rel = new PhotoItagRel { Itag = itag, Photo = photo };
                photo.PhotoItagRel.Add(rel);
            }

            this.db.Photos.Add(photo);
        }

        public IEnumerable<IHumanoidTag> GetAllHumanoidTags<T>() where T : IHumanoidTag
        {
            this.allITags = this.db.Itags.ToList();
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

        public void InsertOrUpdateHumanoidTag(IHumanoidTag hTag)
        {
            var existingHumanoidTag = this.allITags.FirstOrDefault(x => x.Name == hTag.Name && hTag.Posts != 0);
            if (existingHumanoidTag != null)
            {
                if (existingHumanoidTag.Posts == hTag.Posts)
                    return;

                existingHumanoidTag.Posts = hTag.Posts;
                this.db.Itags.Update(existingHumanoidTag);
                this.Save();

                // ToDo this.allITags[key].Posts=hTag.Posts
            }
            else
            {
                var itag = new Itags { Name = hTag.Name, Posts = hTag.Posts };
                this.db.Itags.Add(itag);
                this.Save();
                this.allITags.Add(itag);
            }
        }

    }
}
