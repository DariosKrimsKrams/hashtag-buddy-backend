namespace AutoTagger.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using AutoTagger.API.Models;
    using AutoTagger.Common;
    using AutoTagger.Contract;
    using AutoTagger.Contract.Models;

    using Microsoft.AspNetCore.Mvc;

    using Newtonsoft.Json;

    [Route("[controller]")]
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
                var customer = new Customer();
                customer.CustomerId = "";
                customer.Id = this.customerStorage.Create(customer);
                customer.GenerateHash();
                this.customerStorage.UpdateCustomerId(customer.Id, customer.CustomerId);
                var output = new Dictionary<string, string>();
                output.Add("customerId", customer.CustomerId);
                return this.Ok(output);
            }
            catch (ArgumentException)
            {
                return this.BadRequest();
            }
        }

    }
}
