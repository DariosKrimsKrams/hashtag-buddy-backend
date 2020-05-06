namespace Instaq.API.Extern.Controllers
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Instaq.API.Extern.Models.Requests;
    using Instaq.Contract.Storage;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerStorage customerStorage;

        public CustomerController(
            ICustomerStorage customerStorage
            )
        {
            this.customerStorage = customerStorage;
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(Dictionary<string, string>), 200)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult CreateCustomer()
        {
            try
            {
                var customer = new Customer { CustomerId = "" };
                customer.Id = this.customerStorage.Create(customer);
                customer.GenerateHash();
                this.customerStorage.UpdateCustomerId(customer.Id, customer.CustomerId);
                var output = new Dictionary<string, string> { { "customerId", customer.CustomerId } };
                return this.Ok(output);
            }
            catch (ArgumentException)
            {
                return this.BadRequest();
            }
        }

        [HttpPost("Infos")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 400)]
        public IActionResult AddInfos(AddInfosRequest dto)
        {
            try
            {
                var customerExist = this.customerStorage.Exists(dto.CustomerId);
                if (!customerExist)
                {
                    return this.NotFound();
                }
                this.customerStorage.UpdateInfos(dto.CustomerId, dto.Infos);
                return this.Ok();
            }
            catch (ArgumentException)
            {
                return this.BadRequest();
            }
        }

    }
}
