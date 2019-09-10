using System;
using System.Collections.Generic;
using System.Text;

namespace Instaq.Contract.Models
{
    public interface ILocation
    {
        Int64 Id { get; set; }

        int InstaId { get; set; }

        string Name { get; set; }

        string Slug { get; set; }

        int Lat { get; set; }

        string Lng { get; set; }

        bool HasPublicPage { get; set; }

        string ProfilePicUrl { get; set; }

        DateTime Created { get; set; }
    }
}
