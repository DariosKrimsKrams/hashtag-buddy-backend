using System;
using System.Collections.Generic;

namespace AutoTagger.Database
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string CustomerId { get; set; }
        public string Data { get; set; }
    }
}
