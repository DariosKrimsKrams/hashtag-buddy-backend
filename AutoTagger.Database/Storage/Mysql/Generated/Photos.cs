using System;
using System.Collections.Generic;

namespace AutoTagger.Database
{
    public partial class Photos
    {
        public string LargeUrl { get; set; }
        public string ThumbUrl { get; set; }
        public string Shortcode { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public string User { get; set; }
        public int Follower { get; set; }
        public int Following { get; set; }
        public int Posts { get; set; }
        public long? LocationId { get; set; }
        public DateTime? Uploaded { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }

        public Locations Location { get; set; }
    }
}
