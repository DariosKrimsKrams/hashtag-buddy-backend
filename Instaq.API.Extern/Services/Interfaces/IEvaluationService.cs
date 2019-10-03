using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Instaq.API.Extern.Services.Interfaces
{
    using Instaq.API.Extern.Models.Responses;

    using Microsoft.AspNetCore.Http;

    public interface IEvaluationService
    {
        bool IsCustomerValid(string customerId);

        EvaluateResponse EvaluateFile(string customerId, IFormFile file, HttpRequest request);

    }
}
