using AmbitCRM.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AmbitCRM.Web.Controllers
{
    public class LoginController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7241/api");
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }    

        [HttpPost]
        public JsonResult CheckLogin(LoginModel loginModel)
        {
           
            string requestUri = $"{_client.BaseAddress}/Login/loginApp";

        
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

          
            HttpResponseMessage response = _client.PostAsync(requestUri, content).Result;
            if (response.IsSuccessStatusCode)
            {
             
                var responseData = response.Content.ReadAsStringAsync().Result;
             

                // Deserialize the JSON response to extract user information
               // var userInfo = JsonConvert.DeserializeObject<ResponseModel>(responseData);

                // Store user information in session
                //_httpContextAccessor.HttpContext.Session.SetString("AuthToken", userInfo.auth_token);
                //_httpContextAccessor.HttpContext.Session.SetString("UserKey", userInfo.UserKey);
                //_httpContextAccessor.HttpContext.Session.SetString("EmployeeName", userInfo.EmployeeName);
                //_httpContextAccessor.HttpContext.Session.SetString("Email", userInfo.Email);
                //_httpContextAccessor.HttpContext.Session.SetString("LastLoginTime", userInfo.LastLoginTime.ToString());


                return Json(responseData);
            }
            return Json(response);
        }









    }
}
