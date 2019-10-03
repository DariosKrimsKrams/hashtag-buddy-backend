namespace Instaq.Common
{
    using Instaq.Common.Utils;
    using Instaq.Contract.Models;

    public class Customer : ICustomer
    {
        public string CustomerId { get; set; }

        public int FeedbackCount { get; set; }

        public int Id { get; set; }

        public string Infos { get; set; }

        public int PhotosCount { get; set; }

        public void GenerateHash()
        {
            var input = this.Id + "Instaq";
            this.CustomerId = Hash.GetSha256(input);
        }
    }
}
