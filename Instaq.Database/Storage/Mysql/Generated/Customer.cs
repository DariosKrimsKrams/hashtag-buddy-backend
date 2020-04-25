using System;
using System.Collections.Generic;

namespace Instaq.Database.Storage.Mysql.Generated
{
    public partial class Customer
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int PhotosCount { get; set; }
        public int FeedbackCount { get; set; }
        public int SearchCount { get; set; }
        public string Infos { get; set; }
        public DateTime Created { get; set; }
    }
}
