using System;
using System.Collections.Generic;

namespace AutoTagger.Database.Storage.Mysql.Generated
{
    public partial class Itags
    {
        public Itags()
        {
            PhotoItagRel = new HashSet<PhotoItagRel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Posts { get; set; }
        public DateTime Updated { get; set; }
        public int RefCount { get; set; }
        public sbyte OnBlacklist { get; set; }

        public ICollection<PhotoItagRel> PhotoItagRel { get; set; }
    }
}
