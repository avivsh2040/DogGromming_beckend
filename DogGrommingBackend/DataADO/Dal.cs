using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DogGrommingBackend.Utilities
{
    public class DAL
    {
        private readonly string ? _connectionString;


       
        public DAL()
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = configuration["ConnectionStrings: DefaultConnection"];
            }
        }

        
        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

       
        public async Task CallStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(storedProcedureName, connection);

                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                 throw new InvalidOperationException("Error executing stored procedure", ex);
            }
        }

       
        public async Task<int?> ExecuteScalarAsync(string query, SqlParameter[] parameters)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(query, connection);

                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddRange(parameters);
                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();

               
                return result as int?;
            }
            catch (Exception ex)
            {                
                throw new InvalidOperationException("Error executing scalar query", ex);
            }
        }

        public async Task<SqlDataReader> CallStoredProcedureWithReaderAsync(string storedProcedureName, SqlParameter[] parameters)
        {
            var connection = CreateConnection();
            var command = new SqlCommand(storedProcedureName, connection)
            {
                CommandType = System.Data.CommandType.StoredProcedure
            };
            command.Parameters.AddRange(parameters);
            await connection.OpenAsync();
            return await command.ExecuteReaderAsync();
        }

        public async Task<DataTable> GetDataTableFromStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters)
        {            
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(storedProcedureName, connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddRange(parameters);
                await connection.OpenAsync();

                using var reader = await command.ExecuteReaderAsync();
                var resultTable = new DataTable();
                resultTable.Load(reader);

                return resultTable;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error retrieving Data", ex);
            }
        }





    }
}
