using AmbitCRM.BO.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AmbitCRM.BO.Helper
{
    public class CommonHelper
    {
        public static string GetConnectionString
        {

            get
            {
                return "Server=103.228.83.115,62300;Initial Catalog=Ambit_crm;Persist Security Info=False;User ID=mtfambit;Password=W9@jZ#5qHp2";

            }
        }
        public static void WriteLog(string message)
        {
            //DALCommon log = new DALCommon();
            //var path = log.GetConfigValue("Error_log");

            string ErrorLogDir = @"F:\\ambitapis.cylsys.com\\CRM_Log";

            if (!Directory.Exists(ErrorLogDir))

                Directory.CreateDirectory(ErrorLogDir);
            ErrorLogDir += "\\error1" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";

            using (StreamWriter sr = new StreamWriter(ErrorLogDir, true))

            {
                sr.WriteLine(DateTime.Now.ToString("DD-MM-yyyy HH-mm-ss") + message);
            }
        }


    }
}


