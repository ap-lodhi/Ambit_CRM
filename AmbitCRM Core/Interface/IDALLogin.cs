using AmbitCRM_Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbitCRM_Core.Interface
{
    public interface IDALLogin
    {
        ResponseModel loginUser(LoginModel objmodel ,string Token);
        ResponseModel SaveLoginDetails(LoginModel objmodel, string Token,int empcode, string email);
        ResponseModel LoginDetails(LoginModel objmodel, string token, string empid);
    }
}
