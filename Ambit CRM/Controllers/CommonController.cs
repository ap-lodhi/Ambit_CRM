using AmbitCRM.BO.DAL;
using AmbitCRM_Core.Interface;
using AmbitCRM_Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Ambit_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {

        private readonly IDALCommon _ICommon;
        private readonly IConfiguration _Config;
        public CommonController(IDALCommon ICommon, IConfiguration config)
        {
            _ICommon = ICommon;
            this._Config = config;
        }
        [HttpPost]
        public ActionResult SaveLog(string module, string mes) {
            var res = _ICommon.SaveLog(module, mes);
            return Ok(res);
        }

        [HttpGet]
        public LoginDetails GetLoginDetails(string jwtToken, string empCode, string deviceId)
        {
            LoginDetails loginDetails = new LoginDetails();

            try
            {
                loginDetails = _ICommon.GetLoginDetails(jwtToken, empCode, deviceId);
                return loginDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public static LoginDetails IsTokenActive(LoginDetails loginDetails)
        {
            try
            {
                LoginDetails LoginDetailsNew = new LoginDetails();
                bool isActive = false;

                var CurrCul = CultureInfo.CurrentCulture.Name;
                CultureInfo us = new CultureInfo(CurrCul);
                string format = "dd'/'MM'/'yyyy HH':'mm";

                DateTime curredate = DateTime.Now;
                string year = curredate.Year.ToString();
                string month = curredate.Month.ToString();
                string day = curredate.Day.ToString();
                string hours = curredate.Hour.ToString();
                string minuts = curredate.Minute.ToString();

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

                string cDate = day + "/" + month + "/" + year + " " + hours + ":" + minuts;

                DateTime cdate = DateTime.ParseExact(cDate, format, us);

                DateTime edate = DateTime.ParseExact(loginDetails.end_date, format, us);

                TimeSpan timeDifference = edate - cdate;

                int minutesDifference = (int)timeDifference.TotalMinutes;

                if (minutesDifference > 0)
                {
                    isActive = true;

                    DateTime updatedDate = cdate.AddMinutes(15);
                    string uyear = updatedDate.Year.ToString();
                    string umonth = updatedDate.Month.ToString();
                    string uday = updatedDate.Day.ToString();
                    string uhours = updatedDate.Hour.ToString();
                    string uminuts = updatedDate.Minute.ToString();

                    if (uday.Length == 1)
                    {
                        uday = "0" + uday;
                    }
                    if (umonth.Length == 1)
                    {
                        umonth = "0" + umonth;
                    }
                    if (uhours.Length == 1)
                    {
                        uhours = "0" + uhours;
                    }
                    if (uminuts.Length == 1)
                    {
                        uminuts = "0" + uminuts;
                    }
                    string uDate = uday + "/" + umonth + "/" + uyear + " " + uhours + ":" + uminuts;

                    LoginDetailsNew.is_active = isActive;
                    LoginDetailsNew.id = loginDetails.id;
                    LoginDetailsNew.end_date = uDate;

                    //  responseModel =  _IDALCommon.UpdateLoginDetails(loginDetails.id, uDate);
                }
                else
                {
                    isActive = false;
                    LoginDetailsNew.is_active = isActive;
                }
                return LoginDetailsNew;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
