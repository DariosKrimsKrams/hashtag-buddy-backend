namespace Instaq.API.Extern.Models.Requests
{
    using System.Collections.Generic;

    public class ResultsFeedback
    {
        public string CustomerId { get; set; }

        public int PhotoId { get; set; }
        
        public string Rating { get; set; }

        public List<string> GoodHashtags { get; set; }

        public List<string> BadHashtags { get; set; }

        public string MissingHashtags { get; set; }

        public string Comment { get; set; }
    }
}
