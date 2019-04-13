using System;
using System.Collections.Generic;

namespace AutoTagger.Database
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string CustomerId { get; set; }
        public int DebugId { get; set; }
        public string Data { get; set; }
        public DateTime Created { get; set; }
        public sbyte Deleted { get; set; }

        public Debug Debug { get; set; }
    }
}
