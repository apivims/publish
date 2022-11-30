using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Error;
using WebApi.Models;

namespace WebApi.Repository
{
    public class Test:ITest
    {

        private IConfiguration _configuration;

        private ILog _logger;

        public Test(IConfiguration configuration,ILog logger)
        {
            _configuration = configuration;
            _logger = logger;
        }


        public  List<PatientDetail> GetAllPatientAsync(DateTime fromDate, DateTime toDate)
        {
            List<PatientDetail> list = new List<PatientDetail>();

            try
            {
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                //builder.DataSource = configuration.GetValue<string>("MSSqlConnectionString:DataSource");
                //builder.UserID = configuration.GetValue<string>("MSSqlConnectionString:UserID");
                //builder.Password = configuration.GetValue<string>("MSSqlConnectionString:Password");
                //builder.InitialCatalog = configuration.GetValue<string>("MSSqlConnectionString:InitialCatalog");


                using (SqlConnection connection = new SqlConnection(_configuration.GetValue<string>("MSSqlConnectionString:DevConnection")))
                {
                   // connection.Open();
                    _logger.Information($"Test Data Pool-Start Time{DateTime.Now.ToString("HH:mm:ss")}");
                   
                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 0;
                    command.CommandText = "[dbo].[sp_ReportStd0000_OPD_NMC]";
                    command.Parameters.AddWithValue("@iFromDate", Utility.DateTimeUtil.GetSQLDateTime(fromDate));
                    command.Parameters.AddWithValue("@iToDate", Utility.DateTimeUtil.GetSQLDateTime(toDate));

                    int result = AsyncSqlRequest.Method(connection, command).Result;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            try
                            {
                                PatientDetail patientDetail = new PatientDetail();

                                patientDetail.patient_name = reader.GetString(0);
                                patientDetail.year_of_birth = Convert.ToString(reader.GetValue(1));
                                patientDetail.address = reader.GetString(2);
                                patientDetail.patient_identification_number = reader.GetString(3);
                                patientDetail.patient_identification_proof = reader.GetString(4);
                                patientDetail.uhid_number = reader.GetString(5);
                                patientDetail.patient_mobile_number = reader.GetString(6);
                                patientDetail.transaction_type = Convert.ToString( reader.GetInt32(9));
                                patientDetail.department_visited_name = reader.GetString(10);

                                patientDetail.department_visited_code = Convert.ToString(reader.GetValue(11));

                                var _transDate = reader.GetDateTime(12);
                                patientDetail.datetime_of_transaction = _transDate.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                patientDetail.visit_id = Convert.ToString(reader.GetValue(13));

                                patientDetail.patient_abha_id = "";

                                list.Add(patientDetail);

                            }
                            catch (Exception ex)
                            {

                                _logger.Error("OPD Patient List" + ex.Message);
                            }



                        }


                    }

                    _logger.Information($"test Data pool-end time{DateTime.Now.ToString("HH:mm:ss")},OPD Patient count:{list.Count.ToString() }");

                }
            }
            catch (Exception ex)
            {
                _logger.Information($"test Data Pool-End Time{DateTime.Now.ToString("HH:mm:ss")} ,OPD Patient count:{list.Count.ToString() } ");
                _logger.Error(ex.StackTrace);
                _logger.Error(ex.Message);
            }



            return list;


        }

    }
}
