namespace Instaq.Common
{
    using System.Collections.Generic;
    using Instaq.Contract;
    using Instaq.Contract.Models;

    public class User : IUser
    {
        public string Username { get; set; }

        public int FollowerCount { get; set; }

        public int FollowingCount { get; set; }

        public int PostCount { get; set; }

        public IEnumerable<IImage> Images { get; set; }
    }
}
