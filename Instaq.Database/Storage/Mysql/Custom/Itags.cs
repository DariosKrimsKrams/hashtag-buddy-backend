namespace Instaq.Database.Storage.Mysql.Generated
{
    using Instaq.Common;
    using Instaq.Contract.Models;

    public partial class Itags
    {
        public static Itags FromHumanoidTag(IHumanoidTag hTag)
        {
            return new Itags
            {
                Name        = hTag.Name,
                Posts       = hTag.Posts,
                RefCount    = hTag.RefCount,
                OnBlacklist = hTag.OnBlacklist
            };
        }

        public IHumanoidTag ToHumanoidTag()
        {
            return new HumanoidTag
            {
                Name        = this.Name,
                Posts       = this.Posts,
                RefCount    = this.RefCount,
                OnBlacklist = this.OnBlacklist
            };
        }
    }
}
