using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using DogGrommingBackend.Models;
using DogGrommingBackend.Utilities;
using DogGrommingBackend.Logic;

namespace DogGrommingBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerLogic _customerLogic;
        private readonly ILogger<CustomerLogic> _logger;

        public CustomerController(CustomerLogic customerLogic, ILogger<CustomerLogic> logger)
        {
            _customerLogic = customerLogic;
            _logger = logger;
        }

        [HttpPut("signUp")]
        public async Task<IActionResult> SignUp([FromBody] Customer customerData)
        {

            if (string.IsNullOrEmpty(customerData.FullName) || string.IsNullOrEmpty(customerData.UserName) || string.IsNullOrEmpty(customerData.Password))
            {
                return BadRequest("Invalid input data.");
            }

            if (!Validate.ValidateFullName(customerData.FullName) ||
                !Validate.ValidateUserName(customerData.UserName) ||
                !Validate.ValidatePassword(customerData.Password))
            {
                return BadRequest("Invalid input data.");
            }



            customerData.Password = Encryptor.HashPassword(customerData.Password);

            var success = await _customerLogic.AddCustomerAsync(customerData);
            if (!success)
            {
                return Conflict("Username already exists.");
            }

            return Ok("Customer registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Customer customerData)
        {
            if (string.IsNullOrEmpty(customerData.UserName) || string.IsNullOrEmpty(customerData.Password))
                return BadRequest("Invalid input data.");

            var customer = await _customerLogic.LoginCustomerAsync(customerData.UserName, customerData.Password);

            if (customer == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = JwtHelper.GenerateToken(customer.CustomerId.ToString());

            // set jwt in HttpOnly cookie
            Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,   
                Secure = true,    
                SameSite = SameSiteMode.None, 
                Expires = DateTime.UtcNow.AddDays(1)  
            });

            return Ok(new { Customer = customer });
        }


        [HttpGet("current")]
        public IActionResult GetCurrentCustomer()
        {
            var token = Request.Cookies["auth_token"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { Message = "No authentication token found." });
            }

            var userId = JwtHelper.ValidateAndGetUserId(token);
            if (userId == null)
            {
                return Unauthorized(new { Message = "Invalid or expired token." });
            }


            return Ok(userId.ToString());
        }
    }
}
