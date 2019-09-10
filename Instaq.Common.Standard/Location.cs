namespace Instaq.Common
{
    using System;
    using Instaq.Contract.Models;

    public class Location : ILocation
    {
        public Int64 Id { get; set; }

        public int InstaId { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public int Lat { get; set; }

        public string Lng { get; set; }

        public bool HasPublicPage { get; set; }

        public string ProfilePicUrl { get; set; }

        public DateTime Created { get; set; }
    }
}
