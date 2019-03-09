namespace AutoTagger.Contract.Models
{
    public interface ICustomer
    {
        int Id { get; set; }

        string CustomerId { get; set; }

        int PhotosCount { get; set; }

        void GenerateHash();
    }
}
