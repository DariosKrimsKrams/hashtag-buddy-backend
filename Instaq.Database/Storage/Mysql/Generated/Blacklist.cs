using System;
using System.Collections.Generic;

namespace Instaq.Database
{
    public partial class Blacklist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        public string Table { get; set; }
    }
}
