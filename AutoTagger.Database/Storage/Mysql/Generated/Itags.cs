using System;
using System.Collections.Generic;

namespace AutoTagger.Database.Storage.Mysql.Generated
{
    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

    public partial class Itags
    {
        public Itags()
        {
            PhotoItagRel = new HashSet<PhotoItagRel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Posts { get; set; }
        public DateTime Updated { get; set; }
        public int RefCount { get; set; }
        public sbyte OnBlacklist { get; set; }

        public ICollection<PhotoItagRel> PhotoItagRel { get; set; }

        public IEnumerable<Photos> Photos
        {
            get
            {
                foreach (var photoItagRel in PhotoItagRel)
                {
                    yield return photoItagRel.Photo;
                }
            }
        }

        public IHumanoidTag ToHumanoidTag()
        {
            return new HumanoidTag
            {
                Id       = this.Id,
                Name     = this.Name,
                Posts    = this.Posts,
                RefCount = this.RefCount,
                OnBlacklist = Convert.ToBoolean(this.OnBlacklist)
            };
        }

        public static Itags FromHumanoidTag(IHumanoidTag hTag)
        {
            return new Itags
            {
                Id          = hTag.Id,
                Name        = hTag.Name,
                Posts       = hTag.Posts,
                RefCount    = hTag.RefCount,
                OnBlacklist = Convert.ToSByte(hTag.OnBlacklist)
            };
        }
    }
}
