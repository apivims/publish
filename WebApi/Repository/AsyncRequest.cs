using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Repository
{
    public class AsyncSqlRequest
    {
       public static async Task<int> Method(SqlConnection conn, SqlCommand cmd)
        {
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return 1;
        }
    }
}
