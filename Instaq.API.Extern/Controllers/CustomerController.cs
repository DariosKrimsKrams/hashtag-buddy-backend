namespace Instaq.API.Extern.Controllers
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Contract;
    using Microsoft.AspNetCore.Mvc;

    [Route("[controller]")]
    [Produces("application/json")]
    public class CustomerController : Controller
    {
        private readonly ICustomerStorage customerStorage;

        public CustomerController(
            ICustomerStorage customerStorage
            )
        {
            this.customerStorage = customerStorage;
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(void), 200)]
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

    }
}
