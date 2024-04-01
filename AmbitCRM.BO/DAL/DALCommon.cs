
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmbitCRM_Core.Model;
using AmbitCRM_Core.Interface;
using AmbitCRM.BO.Helper;
using Dapper;



namespace AmbitCRM.BO.DAL
{
    public class DALCommon : IDALCommon
    {
        ResponseModel responseModel = new ResponseModel();
        public ResponseModel SaveLog(string module, string mes)
        {
            ResponseModel result = new ResponseModel();
            using (SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_InsertLog", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Module", module);
                        cmd.Parameters.AddWithValue("@Description", mes);
                        cmd.Parameters.AddWithValue("@CreatedBy", "Admin");

                        var id = cmd.ExecuteNonQuery();

                        if (id > 0)
                        {
                            result.status = true;
                            result.message = "Log Saved Successfully";
                        }
                        else
                        {
                            result.status = false;
                            result.message = "Please Check...Something Went wrong...!!!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.status = false;
                    CommonHelper.WriteLog("ex" + ex);
                    Console.WriteLine("Error is:" + ex);
                }
                finally
                {
                    con.Close();
                }
            }
            return result;
        }


        // Method to retrieve configuration value using stored procedure
        public string GetConfigValue(string key)
        {
            string value = null;


            using (SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_GetConfigValue", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@key", key);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            value = result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while fetching config value: " + ex.Message);
                }
            }

            return value;
        }


        public LoginDetails GetLoginDetails(string jwtToken, string empEmail, string deviceId)
        {
            LoginDetails loginDetails = new LoginDetails();

            try
            {
                using (SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_get_login_details", con))
                    {

                      
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Token", jwtToken);
                        cmd.Parameters.AddWithValue("@EmployeeEmail", empEmail);
                        cmd.Parameters.AddWithValue("@DeviceId", deviceId);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            loginDetails.id = string.IsNullOrEmpty(reader["id"].ToString()) ? 0 : Convert.ToInt32(reader["id"]);
                            loginDetails.token = string.IsNullOrWhiteSpace(reader["Token"].ToString()) ? "" : reader["Token"].ToString();
                            loginDetails.employee_code = string.IsNullOrWhiteSpace(reader["EmployeeEmail"].ToString()) ? "" : reader["EmployeeEmail"].ToString();

                            loginDetails.start_date = string.IsNullOrWhiteSpace(reader["StartDate"].ToString()) ? "" : reader["StartDate"].ToString();

                            loginDetails.end_date = string.IsNullOrWhiteSpace(reader["EndDate"].ToString()) ? "" : reader["EndDate"].ToString();

                            loginDetails.device_id = string.IsNullOrWhiteSpace(reader["DeviceId"].ToString()) ? "" : reader["DeviceId"].ToString();

                            loginDetails.is_active = (bool)reader["IsActive"];




                        }
                        reader.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }

            return loginDetails;
        }



        public ResponseModel UpdateLoginDetails(int id, string uDate)
        {
            ResponseModel responseModel = new ResponseModel();

            try
            {
                int retVal = 0;
                using (SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_update_login_token_details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@udate", uDate);

                    retVal = cmd.ExecuteNonQuery();
                    if (retVal > 0)
                    {
                        responseModel.status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonHelper.WriteLog("ex" + ex);
            }

            return responseModel;
        }



    }
}

