using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Instaq.API.Extern.Services.Interfaces
{
    using Microsoft.AspNetCore.Http;

    public interface IEvaluationService
    {
        bool IsCustomerValid(string customerId);

        Dictionary<string, object> EvaluateImageUpload(string customerId, IFormFile file, HttpRequest request);

    }
}
