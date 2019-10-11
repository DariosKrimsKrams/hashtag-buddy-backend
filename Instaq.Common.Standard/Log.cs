namespace Instaq.Contract.Models
{
    using System;

    public class Log : ILog
    {
        public int Id { get; set; }

        public string Data { get; set; }

        public DateTime Created { get; set; }

        public bool Deleted { get; set; }
    }
}
