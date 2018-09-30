namespace AutoTagger.Database
{
    using System;
    using System.Collections.Generic;
    using AutoTagger.Common;
    using AutoTagger.Contract;

    public partial class Itags
    {
        //public IEnumerable<Photos> Photos
        //{
        //    get
        //    {
        //        foreach (var photoItagRel in PhotoItagRel)
        //        {
        //            yield return photoItagRel.Photo;
        //        }
        //    }
        //}

        public static Itags FromHumanoidTag(IHumanoidTag hTag)
        {
            return new Itags
            {
                Name        = hTag.Name,
                Posts       = hTag.Posts,
                RefCount    = hTag.RefCount,
                OnBlacklist = Convert.ToSByte(hTag.OnBlacklist)
            };
        }

        public IHumanoidTag ToHumanoidTag()
        {
            return new HumanoidTag
            {
                Name        = this.Name,
                Posts       = this.Posts,
                RefCount    = this.RefCount,
                OnBlacklist = Convert.ToBoolean(this.OnBlacklist)
            };
        }
    }
}
