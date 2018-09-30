using System;
using System.Collections.Generic;
using System.Linq;
using AutoTagger.Common;
using AutoTagger.Contract;

namespace AutoTagger.Database
{

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

        //private IEnumerable<Itags> Itags
        //{
        //    get
        //    {
        //        foreach (var photoItagRel in PhotoItagRel)
        //        {
        //            yield return photoItagRel.Itag;
        //        }
        //    }
        //}

        public IImage ToImage()
        {
            var user = new User()
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
                User         = user,
                MachineTags =
                    this.Mtags.Select(tag => new MachineTag { Name = tag.Name, Score = tag.Score, Source = tag.Source }),
                //HumanoidTags = this.Itags.Select(tag => tag.Name)
            };
            return image;
        }
    }
}
