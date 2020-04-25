namespace Instaq.API.Extern.Services.Interfaces
{
    using System.Collections.Generic;
    using Instaq.API.Extern.Models.Responses;
    using Microsoft.AspNetCore.Http;

    public interface IEvaluationService
    {
        bool IsCustomerValid(string customerId);

        EvaluateResponse EvaluateFile(string customerId, IFormFile file, HttpRequest request);

        SearchResponse GetSimilarHashtags(string customerId, string keyword);

        SearchResponse GetSimilarHashtags(string customerId, IEnumerable<string> keywords, IEnumerable<string> excludeHashtags);
        
    }
}
