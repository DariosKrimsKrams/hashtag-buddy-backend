namespace AutoTagger.Database
{
    using System;
    using AutoTagger.Common;
    using AutoTagger.Contract.Models;

    public partial class Itags
    {
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
