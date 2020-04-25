namespace Instaq.Database.Storage.Mysql.Generated
{
    using Instaq.Common;
    using Instaq.Contract.Models;

    public partial class Photos
    {
        public IImage ToImage()
        {
            var user = new User
            {
                FollowerCount  = this.Follower,
                FollowingCount = this.Following,
                PostCount      = this.Posts,
                Username       = this.User,
            };
            var image = new Image
            {
                LargeUrl     = this.LargeUrl,
                ThumbUrl     = this.ThumbUrl,
                Shortcode    = this.Shortcode,
                Likes        = this.Likes,
                CommentCount = this.Comments,
                Created      = this.Created,
                User         = user
            };
            return image;
        }
    }
}
