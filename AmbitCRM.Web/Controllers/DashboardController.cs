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
            _client.DefaultRequestHeaders.Add("login", "login");
            List<SearchModel> srcList = new List<SearchModel>();
            string url = $"{_client.BaseAddress}/Contact/GetSearchDetails?id={1}";
            HttpResponseMessage response = _client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                srcList = JsonConvert.DeserializeObject<List<SearchModel>>(data);
            }
          
            return View(srcList);
        }
        public JsonResult GetContact(DataTableParams dataTableParams, string? companyName, string? contactName, string? email, string? city ,string? SearchType)
        {
            string srctype = SearchType;
            int totalCount = 0;
            int start = dataTableParams.start;
            int length = dataTableParams.length;
            string searchText = dataTableParams.searchText;
            string sortColumn = dataTableParams.sortColumn;
            string sortDirection = dataTableParams.sortDirection;
            List<ContactViewModel> contactList = new List<ContactViewModel>();
            List<ContactViewModel> contactListBookMark = new List<ContactViewModel>();
            List<ContactViewModel> contactListFinal = new List<ContactViewModel>();
            List<ContactViewModel> contactListMain = new List<ContactViewModel>();

            _client.DefaultRequestHeaders.Add("login", "login");
            //  _client.DefaultRequestHeaders.Add("empEmail", "test@gmail.occ");
            //  _client.DefaultRequestHeaders.Add("deviceId", "457645");

            if (!string.IsNullOrEmpty(companyName) || !string.IsNullOrEmpty(contactName) || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(city) || !string.IsNullOrEmpty(SearchType))
            {
               
                string url = $"{_client.BaseAddress}/Contact/ContactList?start={start}&length={length}&searchText={searchText}&sortColumn={sortColumn}&sortDirection={sortDirection}";

                // Append parameters if they are not null or empty
                if (!string.IsNullOrEmpty(companyName))
                    url += $"&CompanyName={companyName}";
                if (!string.IsNullOrEmpty(contactName))
                    url += $"&ContactName={contactName}";
                if (!string.IsNullOrEmpty(email))
                    url += $"&Email={email}";
                if (!string.IsNullOrEmpty(city))
                    url += $"&City={city}"; 
                if (!string.IsNullOrEmpty(SearchType))
                    url += $"&SearchType={SearchType}";

                HttpResponseMessage response = _client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    contactList = JsonConvert.DeserializeObject<List<ContactViewModel>>(data);
                }
            }
            if (contactList.Count > 0)
            {


                totalCount = totalCount + Convert.ToInt32(contactList[0].total_records);

            }
            string id = "1";

            string bookMarkurl = $"{_client.BaseAddress}/Contact/GetBoomarkedList?id={id}&start={start}&length={length}&sortColumn={sortColumn}&sortDirection={sortDirection}"; 
            HttpResponseMessage responsebookmakrd = _client.GetAsync(bookMarkurl).Result;

            if (responsebookmakrd.IsSuccessStatusCode)
            {
                string data = responsebookmakrd.Content.ReadAsStringAsync().Result;
                contactListBookMark = JsonConvert.DeserializeObject<List<ContactViewModel>>(data);
            }

            //contactListFinal.Concat(contactList);
            //contactListFinal.Concat(contactListBookMark);

            totalCount = totalCount + contactListBookMark.Count;


            contactListFinal = contactList.Concat(contactListBookMark).ToList();

            foreach (ContactViewModel contact in contactListFinal)
            {
                ContactViewModel contactModel = new ContactViewModel();

                contactModel.total_records =  contact.total_records = totalCount;

                contactModel.CreatedDate = contact.CreatedDate;
                contactModel.CompanyName = contact.CompanyName;
                contactModel.ContactName = contact.ContactName;
                contactModel.City = contact.City;   
                contactModel.Email = contact.Email;
                contactModel.Source = contact.Source;
                contactModel.RM = contact.RM;
                contactModel.ContactId = contact.ContactId;
                contactModel.LastInteractionDate = contact.LastInteractionDate;
                contactModel.LastInteractionWith = contact.LastInteractionWith;
                contactModel.IsBookMarked = contact.IsBookMarked;

                contactListMain.Add(contactModel);  
            }


            return new JsonResult(new
            {
                iTotalRecords = contactListMain.Count > 0 ? contactListMain[0].total_records : 0,
                iTotalDisplayRecords = contactListMain.Count > 0 ? contactListMain[0].total_records : 0,
                aaData = contactListMain
            });
        }
        [HttpPost]
        public JsonResult BookMarked(BookmarkedModel BM)
        {
            _client.DefaultRequestHeaders.Add("login", "login");

            string requestUri = $"{_client.BaseAddress}/Contact/SaveBookmarked";
            var json = JsonConvert.SerializeObject(BM);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            HttpResponseMessage response = _client.PostAsync(requestUri, content).Result;
            if (response.IsSuccessStatusCode)
            {

                var responseData = response.Content.ReadAsStringAsync().Result;

                return Json(responseData);

            }
            var errorData = new { ErrorMessage = "Failed to save BookMarked." };
            return Json(errorData);
        }

        [HttpPost]
        public JsonResult SaveSearch(SearchModel SM)
        {
            _client.DefaultRequestHeaders.Add("login", "login");
            string requestUri = $"{_client.BaseAddress}/Contact/SaveSearch";
            var json = JsonConvert.SerializeObject(SM);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = _client.PostAsync(requestUri, content).Result;

            if (response.IsSuccessStatusCode)
            {

                var responseData = response.Content.ReadAsStringAsync().Result;

                return Json(responseData);
                
            }
            var errorData = new { ErrorMessage = "Failed to save search." };
            return Json(errorData);
        }

        [HttpPost]
        public JsonResult DeleteSearch(int id)
        {
            _client.DefaultRequestHeaders.Add("login", "login");
            string url = $"{_client.BaseAddress}/Contact/DeleteSearch?id={id}";

            HttpResponseMessage response = _client.DeleteAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseData = response.Content.ReadAsStringAsync().Result;
                return Json(responseData);
            }
            else
            {
               
                var errorData = new { ErrorMessage = "Failed to delete search." };
                return Json(errorData);
            }
        }



    }
}
