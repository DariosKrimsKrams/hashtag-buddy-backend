﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Instaq.Contract
{
    using Instaq.Contract.Models;

    public interface IUser
    {
        string Username { get; set; }

        int FollowerCount { get; set; }

        int FollowingCount { get; set; }

        int PostCount { get; set; }

        IEnumerable<IImage> Images { get; set; }
    }
}
