using System;
using System.Collections.Generic;

namespace Instaq.Database
{
    public partial class Mtags
    {
        public int Id { get; set; }
        public string Shortcode { get; set; }
        public string Name { get; set; }
        public float Score { get; set; }
        public string Source { get; set; }
        public sbyte OnBlacklist { get; set; }
    }
}
