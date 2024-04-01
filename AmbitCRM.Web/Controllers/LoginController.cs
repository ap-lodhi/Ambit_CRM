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
           
            string requestUri = $"{_client.BaseAddress}/Login/login";

        
            var json = JsonConvert.SerializeObject(loginModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

          
            HttpResponseMessage response = _client.PostAsync(requestUri, content).Result;
            if (response.IsSuccessStatusCode)
            {
             
                var responseData = response.Content.ReadAsStringAsync().Result;
            
                return Json(responseData);
            }
            return Json(response);
        }









    }
}
