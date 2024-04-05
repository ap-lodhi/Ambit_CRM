using AmbitCRM.BO.DAL;
using AmbitCRM_Core.Interface;
using AmbitCRM_Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ambit_CRM.Controllers
{
    [Route("api/[controller]")]     
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IDALLogin _ILogin;
        private readonly IConfiguration _Config;
        public LoginController(IDALLogin ILogin, IConfiguration config)
        {
            _ILogin = ILogin;
            this._Config = config;
        }

        //[HttpPost, Route("login")]
        //public ActionResult login(LoginModel login)
        //{
        //    ResponseModel loginDetails = new ResponseModel();
        //    if (!string.IsNullOrWhiteSpace(login.Email) && !string.IsNullOrWhiteSpace(login.Password))

        //    {
        //        bool AdLogin = true;
        //        LoginModel user = new LoginModel();
        //        user.Email = login.Email;
        //        user.Password = login.Password;
             
        //        if (AdLogin == true)
        //        {
        //            string Token = GenerateToken(user);
                  
        //            loginDetails = _ILogin.loginUser(user, Token);
        //            var empcode = loginDetails.UserKey;

        //            var empid = empcode.ToString();
        //            var email = loginDetails.Email; 

        //            var ans  = _ILogin.SaveLoginDetails(user, Token, empcode, email);
        //          //  var res = _ILogin.LoginDetails(Token, empid);
                    
        //            return Ok(loginDetails);
        //        }
        //        else
        //        {
        //            return BadRequest("Invalid EmployeeCode/Password.");
        //        }
        //    }

        //    else
        //    {
        //        return BadRequest("Please Enter Valid EmployeeCode/Password.");
        //    }
        //}


        [HttpPost, Route("loginApp")]
        public ActionResult loginApp(LoginModel login)
        {
            ResponseModel loginDetails = new ResponseModel();
            if (!string.IsNullOrWhiteSpace(login.Email) && !string.IsNullOrWhiteSpace(login.Password) && !string.IsNullOrWhiteSpace(login.DeviceId))

            {
                bool AdLogin = true;
                LoginModel user = new LoginModel();
                user.Email = login.Email;
                user.Password = login.Password;
                user.DeviceId = login.DeviceId; 

                if (AdLogin == true)
                {
                    string Token = GenerateToken(user);

                    loginDetails = _ILogin.loginUser(user, Token);
                    var empcode = loginDetails.UserKey;

                    var empid = empcode.ToString();
                    var email = loginDetails.Email;

                 //   var ans = _ILogin.SaveLoginDetails(user, Token, empcode, email);
                    var res = _ILogin.LoginDetails(user, Token, email);

                    return Ok(loginDetails);
                }
                else
                {
                    return BadRequest("Invalid EmployeeCode/Password.");
                }
            }

            else
            {
                return BadRequest("Please Enter Valid EmployeeCode/Password.");
            }
        }

        private string GenerateToken(LoginModel lg)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            
            var guid = Guid.NewGuid().ToString();

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, lg.Email),
        new Claim("guid", guid) 
    };

            var token = new JwtSecurityToken(_Config["Jwt:Issuer"],
                _Config["Jwt:Audience"],
                claims,
            
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
