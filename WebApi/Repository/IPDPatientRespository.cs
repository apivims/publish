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
    public class IPDPatientRespository : IIPdPatientRepository
    {
        private IConfiguration _configuration;
        private ILog _logger;


        public IPDPatientRespository(IConfiguration configuration ,ILog logger)
        {
            _configuration = configuration;
            _logger = logger;
        }


        public List<PatientDetail> GetAllIPDPatients(DateTime fromDate, DateTime toDate)
        {
            List<PatientDetail> list = new List<PatientDetail>();
            int ipdcount = 0;
            int ipdDischargecount = 0;
            int ipdTransfercount = 0;
            int skiprecord=0;
            try
            {
                _logger.Information($"-----------------------------------------------------------------------");

                _logger.Information($"Transaction-Start Time{DateTime.Now.ToString("HH:mm:ss")}");
                // for IPD Admission
                using (SqlConnection connection = new SqlConnection(_configuration.GetValue<string>("MSSqlConnectionString:DevConnection")))
                {
                    
                    _logger.Information($"IPD Admission Data Pool-Start Time{DateTime.Now.ToString("HH:mm:ss")}");

                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 0;
                    command.CommandText = "[dbo].[sp_ReportStd0000_IPD_NMC]";
                    command.Parameters.AddWithValue("@iFromDate", Utility.DateTimeUtil.GetSQLDateTime(fromDate));
                    command.Parameters.AddWithValue("@iToDate", Utility.DateTimeUtil.GetSQLDateTime(toDate));



                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        skiprecord = 0;
                        while (reader.Read())
                        {
                            ipdcount++;
                            
                            try
                            {

                                PatientDetail patientDetail = new PatientDetail();

                                patientDetail.patient_name = reader.GetString(0);
                                patientDetail.year_of_birth = Convert.ToString(reader.GetValue(1));
                                patientDetail.address = reader.GetString(2);
                                patientDetail.uhid_number = reader.GetString(3);
                                patientDetail.patient_identification_proof = reader.GetString(4);
                                patientDetail.patient_identification_number = reader.GetString(5);
                               
                               
                                
                                patientDetail.patient_mobile_number = reader.GetString(6);
                                patientDetail.transaction_type = Convert.ToString( reader.GetInt32(9));
                               // patientDetail.department_admitted_name = reader.GetString(10);

                                patientDetail.department_visited_name= reader.GetString(10);


                                patientDetail.department_visited_code = Convert.ToString(reader.GetValue(11));

                                var _transDate = reader.GetDateTime(12);
                                patientDetail.datetime_of_transaction = _transDate.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                patientDetail.visit_id = Convert.ToString(reader.GetValue(16));
                                patientDetail.admission_number = reader.GetValue(13).ToString();

                                patientDetail.patient_abha_id = "";

                                if (patientDetail.patient_identification_number == null)
                                {
                                    patientDetail.patient_identification_number = "";
                                }

                                list.Add(patientDetail);

                            }
                            catch (Exception ex)
                            {
                                _logger.Error($"IPD Admission Patient List IPDAdmission ID " + reader.GetValue(13).ToString() + ex.Message);
                                skiprecord++;

                            }









                        }



                        _logger.Information($"IPD Admission Data Pool-End Time{DateTime.Now.ToString("HH:mm:ss")} , IPD Patient Count(Deliver) { (ipdcount- skiprecord).ToString() },IPD Patient Count(Received) {ipdcount.ToString() } ");


                    }


                    connection.Close();


                }


                // for IPD Discharge
                using (SqlConnection connection = new SqlConnection(_configuration.GetValue<string>("MSSqlConnectionString:DevConnection")))
                {

                    _logger.Information($"IPD Discharge Data Pool-Start Time{DateTime.Now.ToString("HH:mm:ss")}");
                   
                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 0;
                    command.CommandText = "[dbo].[sp_ReportStd0000_IPD_Discharge_NMC]";
                    command.Parameters.AddWithValue("@iFromDate", Utility.DateTimeUtil.GetSQLDateTime(fromDate));
                    command.Parameters.AddWithValue("@iToDate", Utility.DateTimeUtil.GetSQLDateTime(toDate));



                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        skiprecord = 0;
                        while (reader.Read())
                        {
                            ipdDischargecount++;
                            
                            try
                            {
                                PatientDetail patientDetail = new PatientDetail();

                                patientDetail.patient_name = reader.GetString(0);
                                patientDetail.year_of_birth = Convert.ToString(reader.GetValue(1));
                                patientDetail.address = reader.GetString(2);
                                patientDetail.uhid_number = reader.GetString(3);
                                patientDetail.patient_identification_number = reader.GetString(5);
                                patientDetail.patient_identification_proof = reader.GetString(4);
                                
                                patientDetail.patient_mobile_number = reader.GetString(6);
                                patientDetail.transaction_type = Convert.ToString( reader.GetInt32(9));
                               // patientDetail.department_admitted_name = reader.GetString(10);

                                patientDetail.department_visited_code = Convert.ToString(reader.GetValue(11));


                               // var _admissiondate = reader.GetDateTime(12);
                                var _transDate = reader.GetDateTime(12);

                               

                                patientDetail.datetime_of_transaction = _transDate.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                               
                                patientDetail.visit_id = Convert.ToString(reader.GetValue(16));
                              
                                patientDetail.admission_number = reader.GetValue(13).ToString();


                                patientDetail.patient_abha_id = "";

                                list.Add(patientDetail);

                            }
                            catch (Exception ex)
                            {
                                _logger.Error("Discharge Patient List ipd admissionlist " + reader.GetValue(13).ToString() + ex.Message);
                                skiprecord++;

                            }


                        }


                        _logger.Information($"IPD Discharge Data Pool-End  Time{DateTime.Now.ToString("HH:mm:ss")}, IPD  Discharge Patient Count(Deliver) { (ipdDischargecount-skiprecord).ToString() },IPD Discharge Patient Count(Received) {ipdDischargecount.ToString() }");


                    }


                    connection.Close();


                }

                // for IPD transfer
                using (SqlConnection connection = new SqlConnection(_configuration.GetValue<string>("MSSqlConnectionString:DevConnection")))
                {
                   
                    _logger.Information($"IPD Transfer Data Pool-Start Time{DateTime.Now.ToString("HH:mm:ss")}");

                    connection.Open();

                    SqlCommand command = new SqlCommand();

                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 0;
                    command.CommandText = "[dbo].[sp_ReportStd0000_IPD_Transfer_NMC]";
                    command.Parameters.AddWithValue("@iFromDate", Utility.DateTimeUtil.GetSQLDateTime(fromDate));
                    command.Parameters.AddWithValue("@iToDate", Utility.DateTimeUtil.GetSQLDateTime(toDate));



                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        skiprecord = 0;
                        while (reader.Read())
                        {
                            ipdTransfercount++;
                           
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
                                patientDetail.transaction_type = Convert.ToString( reader.GetInt32(9));
                              //  patientDetail.department_admitted_name = reader.GetString(10);
                                patientDetail.department_visited_name= reader.GetString(10);
                                
                                patientDetail.department_visited_code = Convert.ToString(reader.GetValue(11));
                                patientDetail.admission_number = reader.GetValue(13).ToString();

                                var _transDate = reader.GetDateTime(12);

                               patientDetail.datetime_of_transaction = _transDate.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                              
                                patientDetail.visit_id = Convert.ToString(reader.GetValue(16));
                             


                                patientDetail.patient_abha_id = "";

                                list.Add(patientDetail);
                            }
                            catch (Exception ex)
                            {

                                _logger.Error("IPD Transfer list ipd Admisson id" + reader.GetValue(13).ToString() + ex.Message);
                                skiprecord++;
                            }

                           
                         
                        }


                        _logger.Information($"IPD Transfer Data Pool-Start Time{DateTime.Now.ToString("HH:mm:ss")} , IPD  Transfer Patient Count(Deliver) { (ipdTransfercount - skiprecord).ToString() },IPD Transfer Patient Count(Received) {ipdTransfercount.ToString() }");

                    }


                    connection.Close();


                }

                _logger.Information($"Transaction-End Time{DateTime.Now.ToString("HH:mm:ss")},Total Ipd Patient count(Deliver) :{list.Count.ToString()},Total Ipd Patient count(Received) :{(ipdcount+ipdDischargecount+ipdTransfercount ).ToString()}");
            }
           
            catch (Exception ex)
            {
                _logger.Information($"Transaction-End Time{DateTime.Now.ToString("HH:mm:ss")} ,Total Ipd Patient count(Deliver) :{list.Count.ToString()},Total Ipd Patient count(Received) :{(ipdcount + ipdDischargecount + ipdTransfercount).ToString()}");
                _logger.Error(ex.StackTrace);
                _logger.Error(ex.Message);
            }


            _logger.Information("-----------------------------------------------------------------------");

            return list;


        }
    }
}
