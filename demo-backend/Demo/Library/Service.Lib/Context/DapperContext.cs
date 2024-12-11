using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lib.Context
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext()
        {
            _connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
