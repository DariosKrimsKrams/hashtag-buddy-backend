namespace AutoTagger.Contract
{
    using System;
    using System.Collections.Generic;

    using AutoTagger.Contract.Models;

    public interface IImage
    {
        int CommentCount { get; set; }

        IEnumerable<string> Comments { get; set; }

        int Follower { get; set; }

        int Following { get; set; }

        IEnumerable<string> HumanoidTags { get; set; }

        int Id { get; set; }

        string LargeUrl { get; set; }

        int Likes { get; set; }

        IEnumerable<IMachineTag> MachineTags { get; set; }

        int Posts { get; set; }

        string Shortcode { get; set; }

        string ThumbUrl { get; set; }

        DateTime Uploaded { get; set; }

        string User { get; set; }

        ILocation Location { get; set; }
    }
}
