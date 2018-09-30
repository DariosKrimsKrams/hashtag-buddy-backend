using System;
using System.Collections.Generic;

namespace AutoTagger.Database
{
    public partial class PhotoItagRel
    {
        public int Id { get; set; }
        public string Shortcode { get; set; }
        public string Itag { get; set; }
    }
}
