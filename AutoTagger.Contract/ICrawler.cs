﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTagger.Contract
{
    public interface ICrawler
    {
        ICrawlerImage GetCrawlerImageForImageId(string imageId);
    }
}