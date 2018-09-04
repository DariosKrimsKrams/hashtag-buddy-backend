namespace AutoTagger.Common
{
    using System;
    using AutoTagger.Contract.Models;

    public class Location : ILocation
    {
        public int Id { get; set; }

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
