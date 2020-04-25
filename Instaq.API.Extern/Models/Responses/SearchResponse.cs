namespace Instaq.API.Extern.Models.Responses
{
    using System.Collections.Generic;
    using Instaq.Contract.Models;

    public class SearchResponse
    {
        public int LogId { get; set; }

        public IList<IHumanoidTag> Hashtags { get; set; }

        public SearchResponse()
        {
            this.Hashtags = new List<IHumanoidTag>();
        }

    }
}
