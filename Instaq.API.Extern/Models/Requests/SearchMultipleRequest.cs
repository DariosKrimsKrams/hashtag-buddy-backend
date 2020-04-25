namespace Instaq.API.Extern.Models.Requests
{
    using System.Collections.Generic;

    public class SearchMultipleRequest
    {
        public string CustomerId { get; set; }

        public List<string> Keywords { get; set; }

        public IEnumerable<string> ExcludeHashtags { get; set; }
    }
}
