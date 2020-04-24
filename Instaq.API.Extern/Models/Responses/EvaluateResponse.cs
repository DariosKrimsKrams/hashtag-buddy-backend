namespace Instaq.API.Extern.Models.Responses
{
    using System.Collections.Generic;
    using Instaq.Contract.Models;

    public class EvaluateResponse
    {
        public string Img { get; set; }

        public int LogId { get; set; }

        public IEnumerable<IHumanoidTag> MostRelevantHTags { get; set; }

        public IEnumerable<IHumanoidTag> TrendingHTags { get; set; }

        public EvaluateResponse()
        {
            this.Img = "";
            this.MostRelevantHTags = new List<IHumanoidTag>();
            this.TrendingHTags = new List<IHumanoidTag>();
        }

    }
}
