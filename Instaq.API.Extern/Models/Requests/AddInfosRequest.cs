namespace Instaq.API.Extern.Models.Requests
{
    public class AddInfosRequest
    {
        public string CustomerId { get; set; }

        public string Infos { get; set; }

        public AddInfosRequest()
        {
            this.CustomerId = "";
            this.Infos = "";
        }

    }
}
