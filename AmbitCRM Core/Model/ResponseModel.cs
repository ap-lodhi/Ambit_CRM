using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbitCRM_Core.Model
{
    public class ResponseModel
    {
        public bool status { get; set; }
        public string? message { get; set; }
        public string? auth_token { get; set; }
        public int UserKey { get; set; }
        public string? EmployeeName { get; set; }
        public string? Email { get; set; }
        public DateTime LastLoginTime { get; set; } 
    }


    public class LoginDetails
    {
        public int id { get; set; }
        public string? employee_code { get; set; }
        public string? start_date { get; set; }
        public string? end_date { get; set; }
        public string? token { get; set; }
        public string? device_id { get; set; }
        public bool is_active { get; set; }

    }


    public class HeadersKeyDetails
    {
        public string jwtToken { get; set; }
        public string empCode { get; set; }
        public string empEmail { get; set; }
        public string deviceId { get; set; }
        public string loginHeader { get; set; }

    }
}
