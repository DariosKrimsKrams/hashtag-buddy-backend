﻿namespace AutoTagger.Contract
{
    using System;
    using System.Collections.Generic;
    using AutoTagger.Contract.Models;

    public interface IImage
    {
        int CommentCount { get; set; }

        IEnumerable<string> Comments { get; set; }

        IEnumerable<string> HumanoidTags { get; set; }

        string LargeUrl { get; set; }

        int Likes { get; set; }

        ILocation Location { get; set; }

        IEnumerable<IMachineTag> MachineTags { get; set; }

        string Message { get; set; }

        string Shortcode { get; set; }

        string ThumbUrl { get; set; }

        DateTime Uploaded { get; set; }

        DateTime Created { get; set; }

        IUser User { get; set; }
    }
}
