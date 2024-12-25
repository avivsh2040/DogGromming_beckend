using DogGrommingBackend.Models;
using DogGrommingBackend.Utilities;
using Microsoft.EntityFrameworkCore;
using DogGrommingBackend.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace DogGrommingBackend.Logic
{
    public class CustomerLogic
    {
        private readonly EntityDbContext _context;
        private readonly ILogger<CustomerLogic> _logger;

        public CustomerLogic(EntityDbContext context, ILogger<CustomerLogic> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            try
            {
                if (await _context.Customers.AnyAsync(c => c.UserName == customer.UserName))
                {
                    _logger.LogWarning("Attempt to add customer with duplicate username: {UserName}", customer.UserName);
                    return false; // Username already exists
                }

                customer.CreateDate = DateTime.Now;
                customer.BranchId = 1;// to do enum
                
                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Customer added successfully: {UserName}", customer.UserName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding customer");
                return false;
            }
        }

        // not in use
        public async Task<bool> AddCustomerWithStoredProcedureAsync(Customer customer)
        {
            try
            {
                if (await _context.Customers.AnyAsync(c => c.UserName == customer.UserName))
                {
                    _logger.LogWarning("Customer already exists");
                    return false; // Username already exists
                }

                var parameters = new[]
                {
                    new SqlParameter("@UserName", customer.UserName),
                    new SqlParameter("@PasswordHash", customer.Password),
                    new SqlParameter("@CreatedDate", customer.CreateDate),
                };

                await _context.Database.ExecuteSqlRawAsync("EXEC AddCustomer @UserName, @PasswordHash, @CreatedDate", parameters);
                _logger.LogInformation("Customer added successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding customer");
                return false;
            }
        }

        
        public async Task<Customer?> LoginCustomerAsync(string userName, string password)
        {
            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(c => c.UserName == userName);

                if (customer != null)
                {

                    if (string.IsNullOrEmpty(customer.Password))
                        _logger.LogWarning("Invalid input data.");

                    if (!string.IsNullOrEmpty(customer.Password))
                    {

                        if (customer == null || !Encryptor.VerifyPassword(password, customer.Password))
                        {
                            _logger.LogWarning("Invalid login attempt");
                            return null;
                        }
                    }

                }


                _logger.LogInformation("Customer logged in successfully");
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in");
                return null;
            }
        }
    }
}
