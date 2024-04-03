using AmbitCRM.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;

namespace AmbitCRM.Web.Controllers
{
    public class DashboardController : Controller
    {
        //get this from DB app configuration
        Uri baseAddress = new Uri("https://localhost:7241/api");
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DashboardController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            return View();
        }

            //string userKey = _httpContextAccessor.HttpContext.Session.GetString("UserKey");
            //string employeeName = _httpContextAccessor.HttpContext.Session.GetString("EmployeeName");
            //string email = _httpContextAccessor.HttpContext.Session.GetString("Email");
            //DateTime lastLoginTime;
            //string lastLoginTimeString = _httpContextAccessor.HttpContext.Session.GetString("LastLoginTime");

            //ViewBag.emp = employeeName;

        [HttpGet]
        public ActionResult Dashboard()

        {
            List<SearchModel> srcList = new List<SearchModel>();
            string url = $"{_client.BaseAddress}/Contact/SearchDetails?id={1}";
            HttpResponseMessage response = _client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                srcList = JsonConvert.DeserializeObject<List<SearchModel>>(data);
            }
            ViewBag.srch=   srcList;
            return View(srcList);
        }
        public JsonResult GetContact(DataTableParams dataTableParams, string? companyName, string? contactName, string? email, string? city)
        {
            int start = dataTableParams.start;
            int length = dataTableParams.length;
            string searchText = dataTableParams.searchText;
            string sortColumn = dataTableParams.sortColumn;
            string sortDirection = dataTableParams.sortDirection;
            List<ContactViewModel> contactList = new List<ContactViewModel>();

          //  _client.DefaultRequestHeaders.Add("jwtToken", "jrttkennnn");
          //  _client.DefaultRequestHeaders.Add("empEmail", "test@gmail.occ");
          //  _client.DefaultRequestHeaders.Add("deviceId", "457645");

            


            string url = $"{_client.BaseAddress}/Contact/ContactList?start={start}&length={length}&searchText={searchText}&sortColumn={sortColumn}&sortDirection={sortDirection}&CompanyName={companyName}&ContactName={contactName}&Email={email}&City={city}";
            HttpResponseMessage response = _client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                contactList = JsonConvert.DeserializeObject<List<ContactViewModel>>(data);
            }
            return new JsonResult(new
            {
                iTotalRecords = contactList.Count > 0 ? contactList[0].total_records : 0,
                iTotalDisplayRecords = contactList.Count > 0 ? contactList[0].total_records : 0,
                aaData = contactList
            });
        }
        [HttpPost]
        public JsonResult BookMarked(BookmarkedModel BM)
        {
            string requestUri = $"{_client.BaseAddress}/Contact/BookmarkedStatus";
            var json = JsonConvert.SerializeObject(BM);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            HttpResponseMessage response = _client.PostAsync(requestUri, content).Result;
            if (response.IsSuccessStatusCode)
            {

                var responseData = response.Content.ReadAsStringAsync().Result;

                return Json(responseData);

            }
            return Json(response);
        }

        [HttpPost]
        public JsonResult SaveSearch(SearchModel SM)
        {
            string requestUri = $"{_client.BaseAddress}/Contact/SaveSearch";
            var json = JsonConvert.SerializeObject(SM);
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
