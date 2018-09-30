using System;
using System.Collections.Generic;

namespace AutoTagger.Database
{
    public partial class Locations
    {
        public int Id { get; set; }
        public int InstaId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Lat { get; set; }
        public string Lng { get; set; }
        public sbyte HasPublicPage { get; set; }
        public string ProfilePicUrl { get; set; }
        public DateTime Created { get; set; }
    }
}
