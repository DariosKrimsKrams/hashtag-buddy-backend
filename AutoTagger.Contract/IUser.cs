﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Contract
{
    public interface IUser
    {
        string Username { get; set; }

        int FollowerCount { get; set; }

        int FollowingCount { get; set; }

        int PostCount { get; set; }

        IEnumerable<IImage> Images { get; set; }
    }
}
