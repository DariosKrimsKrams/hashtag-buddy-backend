namespace Instaq.Contract.Models
{
    using System;

    public interface ILog
    {
        int Id { get; set; }

        string Data { get; set; }

        DateTime Created { get; set; }

        bool Deleted { get; set; }
    }
}
