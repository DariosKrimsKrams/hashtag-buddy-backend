﻿namespace Instaq.Common
{
    using Instaq.Contract.Models;

    public class Feedback : IFeedback
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string CustomerId { get; set; }

        public int DebugId { get; set; }

        public string Data { get; set; }
    }
}
