using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using WebApi.Error;
using System.Globalization;

namespace WebApi.Repository
{
    public class OpdPatientRepository : IOpdPatientRepository
    {

        private IConfiguration _configuration;

        private ILog _logger;

        public OpdPatientRepository(IConfiguration configuration, ILog logger)
        {
            _configuration = configuration;
            _logger = logger;

        }


        public   List<PatientDetail> GetAllOPDPatients(DateTime fromDate,DateTime toDate)
        {
           
            
            List<PatientDetail> list = new List<PatientDetail>();

            int count =0;

            try
            {
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                //builder.DataSource = configuration.GetValue<string>("MSSqlConnectionString:DataSource");
                //builder.UserID = configuration.GetValue<string>("MSSqlConnectionString:UserID");
                //builder.Password = configuration.GetValue<string>("MSSqlConnectionString:Password");
                //builder.InitialCatalog = configuration.GetValue<string>("MSSqlConnectionString:InitialCatalog");


                using (SqlConnection connection = new SqlConnection(_configuration.GetValue<string>("MSSqlConnectionString:DevConnection")))
                {

                    _logger.Information($"Opd Data Pool-Start Time{DateTime.Now.ToString("HH:mm:ss")}");
                    connection.Open();
                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 0;
                    command.CommandText = "[dbo].[sp_ReportStd0000_OPD_NMC]";
                    command.Parameters.AddWithValue("@iFromDate", Utility.DateTimeUtil.GetSQLDateTime( fromDate));
                    command.Parameters.AddWithValue("@iToDate", Utility.DateTimeUtil.GetSQLDateTime(toDate));



                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            count++;

                            try
                            {
                                PatientDetail patientDetail = new PatientDetail();

                                patientDetail.patient_name = reader.GetString(0);
                                patientDetail.year_of_birth = Convert.ToString(reader.GetValue(1));
                                patientDetail.address = reader.GetString(2);
                                patientDetail.patient_identification_number = reader.GetString(5);
                                patientDetail.patient_identification_proof = reader.GetString(4);
                                patientDetail.uhid_number = reader.GetString(3);
                                patientDetail.patient_mobile_number = reader.GetString(6);
                                patientDetail.transaction_type = Convert.ToString(  reader.GetInt32(9));
                                patientDetail.department_visited_name = reader.GetString(10);
                                 
                                patientDetail.department_visited_code = Convert.ToString(reader.GetValue(11));

                                var _transDate = reader.GetDateTime(12);
                                DateTime Date1 = reader.GetDateTime(12);

                                 // var test2 = DateTime.ParseExact("2022-08-06 12:18:30", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                //string k = test2.ToString("dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture);

                                //string _date=_transDate.ToString()

                                // DateTime validDate = DateTime.ParseExact(_transDate, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                                patientDetail.datetime_of_transaction = _transDate.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                               
                                patientDetail.visit_id = Convert.ToString(reader.GetValue(13));

                                patientDetail.patient_abha_id = "";
                                patientDetail.admission_number = "";

                                list.Add(patientDetail);

                            }
                            catch (Exception ex)
                            {

                                _logger.Error("OPD Patient List visit id" + Convert.ToString(reader.GetValue(13)) + ex.Message);
                            }

                    
                       
                        }
                   
                    
                    }

                   _logger.Information($"Opd Data pool-end time{DateTime.Now.ToString("HH:mm:ss")},OPD Patient count(Delivered):{list.Count.ToString() } ,OPD Patient count(Received):{count.ToString() }");

                }
            }
            catch (Exception ex)
            {
                _logger.Information($"Opd Data Pool-End Time{DateTime.Now.ToString("HH:mm:ss")} ,OPD Patient count(Delivered):{list.Count.ToString() },OPD Patient count(Received):{count.ToString() } ");
                _logger.Error(ex.StackTrace);
                _logger.Error(ex.Message);
            }




            return list;


            //throw new NotImplementedException();
        }


    }
}
