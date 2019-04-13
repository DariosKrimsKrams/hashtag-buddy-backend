using System;
using System.Collections.Generic;

namespace AutoTagger.Database
{
    public partial class Debug
    {
        public Debug()
        {
            Feedback = new HashSet<Feedback>();
        }

        public int Id { get; set; }
        public string Data { get; set; }
        public string CustomerId { get; set; }
        public DateTime Created { get; set; }
        public sbyte Deleted { get; set; }

        public ICollection<Feedback> Feedback { get; set; }
    }
}
