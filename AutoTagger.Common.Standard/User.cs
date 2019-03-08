using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Common
{
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

    public class User : IUser
    {
        public string Username { get; set; }

        public int FollowerCount { get; set; }

        public int FollowingCount { get; set; }

        public int PostCount { get; set; }

        public IEnumerable<IImage> Images { get; set; }
    }
}
