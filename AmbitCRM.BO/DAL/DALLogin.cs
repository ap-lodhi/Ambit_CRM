using AmbitCRM.BO.Helper;

using AmbitCRM_Core.Interface;
using AmbitCRM_Core.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AmbitCRM.BO.DAL
{
    public class DALLogin : IDALLogin
    {
        DALCommon log = new DALCommon();
        public ResponseModel loginUser(LoginModel objmodel, string Token)
        {
            ResponseModel result = new ResponseModel();
            
            using (SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("ValidateLoginUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", objmodel.Email);
                        cmd.Parameters.AddWithValue("@Password", objmodel.Password);

                        // Add output parameters
                        SqlParameter employeeCodeParam = new SqlParameter("@UserKey", SqlDbType.Int);
                        SqlParameter employeeNameParam = new SqlParameter("@EmployeeName", SqlDbType.NVarChar, 50);
                        SqlParameter lastLoginDateParam = new SqlParameter("@LastLoginDate", SqlDbType.DateTime);
                        employeeCodeParam.Direction = ParameterDirection.Output;
                        employeeNameParam.Direction = ParameterDirection.Output;
                        lastLoginDateParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(employeeCodeParam);
                        cmd.Parameters.Add(employeeNameParam);
                        cmd.Parameters.Add(lastLoginDateParam);
                        cmd.ExecuteNonQuery(); // Execute the stored procedure

                        int employeeCode = (int)employeeCodeParam.Value;
                        string employeeName = employeeNameParam.Value.ToString();
                        DateTime lastLoginDate = (DateTime)lastLoginDateParam.Value;

                        if (employeeCode != 0)
                        {
                            result.status = true;
                            result.message = "Login successfull.";
                            result.auth_token = Token;
                            result.Email = objmodel.Email;
                            result.UserKey = employeeCode;
                            result.EmployeeName = employeeName;
                            result.LastLoginTime = lastLoginDate;
                        }
                        else
                        {
                            result.status = false;
                            result.message = "Invalid email or password.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.status = false;

                    log.SaveLog("Login User", "ex" + ex);

                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            return result;
        }


        public ResponseModel SaveLoginDetails(LoginModel objmodel, string Token, int empcode, string email)
        {
            ResponseModel result = new ResponseModel();
         
            using (SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString))
            {
                try
                {
                    con.Open();

                    DateTime curredate = DateTime.Now;
                    DateTime ecurredate = DateTime.Now.AddMinutes(15);
                    string ipAddress = GetIpAddress();

                    using (SqlCommand cmd = new SqlCommand("Sp_Insert_Login_Details", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@userKey", empcode);
                        cmd.Parameters.AddWithValue("@Token", Token);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@StartDate", curredate);
                        cmd.Parameters.AddWithValue("@EndDate", ecurredate);
                        cmd.Parameters.AddWithValue("@DeviceId", ipAddress);
                        cmd.Parameters.AddWithValue("@CreatedBy", empcode);


                        var id = cmd.ExecuteNonQuery();

                        if (id > 0)
                        {
                            result.status = true;
                            result.message = "Login Details Save";
                            log.SaveLog("save Login Details", "Login Details Save Successfully");
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
                    log.SaveLog("save LoginDetails", "ex" + ex);

                }
                finally
                {
                    con.Close();
                }
            }
            return result;
        }

        private string GetIpAddress()
        {
            var httpContext = new HttpContextAccessor().HttpContext;

            // Get IP Address of the client
            string ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            return ipAddress;
        }


        public ResponseModel LoginDetails(LoginModel objmodel ,string token, string empid)
        {
            ResponseModel response = new ResponseModel();       
            try
            {
                var CurrCul = CultureInfo.CurrentCulture.Name;
                CultureInfo us = new CultureInfo(CurrCul);
                string format = "dd'/'MM'/'yyyy HH':'mm";

                DateTime curredate = DateTime.Now;
                DateTime ecurredate = DateTime.Now.AddMinutes(15);
                string year = curredate.Year.ToString();
                string month = curredate.Month.ToString();
                string day = curredate.Day.ToString();
                string hours = curredate.Hour.ToString();
                string minuts = curredate.Minute.ToString();


                string eyear = ecurredate.Year.ToString();
                string emonth = ecurredate.Month.ToString();
                string eday = ecurredate.Day.ToString();
                string ehours = ecurredate.Hour.ToString();
                string eminuts = ecurredate.Minute.ToString();

                if (eday.Length == 1)
                {
                    eday = "0" + eday;
                }
                if (emonth.Length == 1)
                {
                    emonth = "0" + emonth;
                }
                if (ehours.Length == 1)
                {
                    ehours = "0" + ehours;
                }
                if (eminuts.Length == 1)
                {
                    eminuts = "0" + eminuts;
                }


                if (day.Length == 1)
                {
                    day = "0" + day;
                }
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                if (hours.Length == 1)
                {
                    hours = "0" + hours;
                }
                if (minuts.Length == 1)
                {
                    minuts = "0" + minuts;
                }
               string sDate = day + "/" + month + "/" + year + " " + hours + ":" + minuts;

               string eDate = eday + "/" + emonth + "/" + eyear + " " + ehours + ":" + eminuts;
              
                using (SqlConnection con = new SqlConnection(CommonHelper.GetConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Sp_save_token_login_details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmployeeEmail", empid);
                    cmd.Parameters.AddWithValue("@Token", token);
                    cmd.Parameters.AddWithValue("@StartDate", sDate);
                    cmd.Parameters.AddWithValue("@EndDate", eDate);
                    cmd.Parameters.AddWithValue("@DeviceId", objmodel.DeviceId);
                    cmd.Parameters.AddWithValue("@CreatedBy", empid);
                    var  retVal = cmd.ExecuteNonQuery();
                    if (retVal >0)
                    {
                        response.status = true;
                        log.SaveLog("save Login Details New One", "Login Details Save Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                log.SaveLog("Login User", "ex" + ex);
            }
            return response;
        }
    }
}








