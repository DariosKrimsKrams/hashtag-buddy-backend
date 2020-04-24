namespace Instaq.API.Extern.Models.Responses
{
    using System.Collections.Generic;
    using Instaq.Contract.Models;

    public class SearchResponse
    {
        public IEnumerable<IHumanoidTag> Hashtags { get; set; }

        public SearchResponse()
        {
            this.Hashtags = new List<IHumanoidTag>();
        }

    }
}
