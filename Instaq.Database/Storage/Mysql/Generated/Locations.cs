﻿using System;
using System.Collections.Generic;

namespace Instaq.Database.Storage.Mysql.Generated
{
    public partial class Locations
    {
        public Locations()
        {
            Photos = new HashSet<Photos>();
        }

        public long Id { get; set; }
        public int InstaId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Lat { get; set; }
        public string Lng { get; set; }
        public bool HasPublicPage { get; set; }
        public string ProfilePicUrl { get; set; }
        public DateTime Created { get; set; }

        public virtual ICollection<Photos> Photos { get; set; }
    }
}
