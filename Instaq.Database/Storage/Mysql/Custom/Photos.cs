using Instaq.Common;

namespace Instaq.Database.Storage.Mysql.Generated
{
    using Instaq.Contract.Models;

    public partial class Photos
    {
        public static Photos FromImage(IImage image)
        {
            var photo = new Photos
            {
                LargeUrl       = image.LargeUrl,
                ThumbUrl       = image.ThumbUrl,
                Shortcode = image.Shortcode,
                Likes     = image.Likes,
                Comments  = image.CommentCount,
                User      = image.User.Username,
                Follower = image.User.FollowerCount,
                Following  = image.User.FollowingCount,
                Posts  = image.User.PostCount,
                Uploaded  = image.Uploaded
            };

            return photo;
        }

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
