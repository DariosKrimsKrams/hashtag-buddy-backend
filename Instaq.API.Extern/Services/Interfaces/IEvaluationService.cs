namespace Instaq.API.Extern.Services.Interfaces
{
    using System.Collections.Generic;
    using Instaq.API.Extern.Models.Responses;
    using Instaq.Contract.Models;

    using Microsoft.AspNetCore.Http;

    public interface IEvaluationService
    {
        bool IsCustomerValid(string customerId);

        EvaluateResponse EvaluateFile(string customerId, IFormFile file, HttpRequest request);

        IEnumerable<IHumanoidTag> GetSimilarHashtags(string keyword);
        
    }
}
