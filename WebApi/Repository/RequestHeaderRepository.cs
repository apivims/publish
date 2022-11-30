using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Error;
using WebApi.Model_hims;

namespace WebApi.Repository
{
    public class RequestHeaderRepository : IRequestHeaderRepository
    {
        private IConfiguration _configuration;
        private ILog _logger;

        public RequestHeaderRepository(IConfiguration configuration , ILog logger)
        {
            _configuration = configuration;
            _logger = logger;
        }


        public string AddRequestHeader(RequestHeader requestHeader)
        {
            // throw new NotImplementedException();

            long requestid = 0;

            try
            {
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                //builder.DataSource = configuration.GetValue<string>("MSSqlConnectionString:DataSource");
                //builder.UserID = configuration.GetValue<string>("MSSqlConnectionString:UserID");
                //builder.Password = configuration.GetValue<string>("MSSqlConnectionString:Password");
                //builder.InitialCatalog = configuration.GetValue<string>("MSSqlConnectionString:InitialCatalog");


                using (SqlConnection connection = new SqlConnection(_configuration.GetValue<string>("MSSqlConnectionString:DevConnection")))
                {


                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[dbo].[sp_AddRequestHeader_NMC]";
                    command.Parameters.AddWithValue("@requestDate",Utility.DateTimeUtil.GetSQLDateTime(DateTime.Now));
                    command.Parameters.AddWithValue("@hfidABDM",requestHeader.hfidABDM);
                    command.Parameters.AddWithValue("@hfidHMIS",requestHeader.hfidHMIS);
                    command.Parameters.AddWithValue("@fromDate",requestHeader.fromDate);
                    command.Parameters.AddWithValue("@toDate",requestHeader.toDate);
                    command.Parameters.AddWithValue("@syncRequestType", requestHeader.syncRequestType);
                    command.Parameters.AddWithValue("@txn_id",requestHeader.txn_id);
                    command.Parameters.AddWithValue("@module_code",requestHeader.module_code);
                    command.Parameters.AddWithValue("@Authorization",requestHeader.Authorization);
                    command.Parameters.AddWithValue("@msg", requestHeader.msg);
                    command.Parameters.Add("@id", SqlDbType.BigInt);
                    command.Parameters["@id"].Direction = ParameterDirection.Output;
                     int i= command.ExecuteNonQuery();
                     requestid =  Convert.ToInt64(command.Parameters["@id"].Value);

                }

                return requestid.ToString();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }


            return requestid.ToString();

        }
    }
}
