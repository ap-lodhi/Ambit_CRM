using AmbitCRM_Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbitCRM_Core.Interface
{
    public interface IDALCommon
    {
        ResponseModel SaveLog(string module, string mes);
        LoginDetails GetLoginDetails(string jwtToken, string empCode, string deviceId);
        
    }
}
