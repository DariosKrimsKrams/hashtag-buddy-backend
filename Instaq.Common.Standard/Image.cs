﻿namespace Instaq.Common
{
    using System;
    using System.Collections.Generic;
    using Instaq.Contract;
    using Instaq.Contract.Models;

    public class Image : IImage
    {
        public int CommentCount { get; set; }

        public IEnumerable<string> Comments { get; set; }

        public IEnumerable<string> HumanoidTags { get; set; }

        public string LargeUrl { get; set; }

        public int Likes { get; set; }

        public ILocation Location { get; set; }

        public IEnumerable<IMachineTag> MachineTags { get; set; }

        public string Message { get; set; }

        public string Shortcode { get; set; }

        public string ThumbUrl { get; set; }

        public DateTime Uploaded { get; set; }

        public DateTime Created { get; set; }

        public IUser User { get; set; }

        public string Status { get; set; }

    }
}
