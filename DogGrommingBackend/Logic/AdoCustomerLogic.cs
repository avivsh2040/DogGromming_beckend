using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DogGrommingBackend.Models;
using DogGrommingBackend.Utilities;
using Microsoft.Data.SqlClient;

namespace DogGrommingBackend.Logic
{
    public class AdoCustomerLogic
    {
        private readonly DAL _dal;

        
        public AdoCustomerLogic(DAL dal)
        {
            _dal = dal;
        }

        
        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            //check if the username already exists
            var parameters = new[]
            {
                new SqlParameter("@UserName", customer.UserName),
                new SqlParameter("@PasswordHash", customer.Password),
                new SqlParameter("@CreatedDate", customer.CreateDate)
            };
                      
            await _dal.CallStoredProcedureAsync("AddCustomer", parameters);
            return true;
        }

        
        public async Task<bool> AddCustomerWithStoredProcedureAsync(Customer customer)
        {
            var parameters = new[]
            {
                new SqlParameter("@UserName", customer.UserName),
                new SqlParameter("@PasswordHash", customer.Password),
                new SqlParameter("@CreatedDate", customer.CreateDate)
            };

           
            await _dal.CallStoredProcedureAsync("AddCustomer", parameters);
            return true;
        }

       
        public async Task<Customer?> LoginAsync(string userName, string password)
        {
            var parameters = new[]
            {
                new SqlParameter("@UserName", userName),
                new SqlParameter("@Password", password)
            };
                        
            using (var result = await _dal.CallStoredProcedureWithReaderAsync("GetCustomer", parameters))
            {
              
                if (result.HasRows)
                {
                    result.Read();
                    return new Customer
                    {
                        UserName =   result["UserName"].ToString(),
                        CreateDate = Convert.ToDateTime(result["CreatedDate"])
                    };
                }
            }

            return null;
        }

       
    }
}
