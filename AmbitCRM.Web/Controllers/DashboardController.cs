using AmbitCRM.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;

namespace AmbitCRM.Web.Controllers
{
    public class DashboardController : Controller
    {
        //get this from DB app configuration
        Uri baseAddress = new Uri("https://localhost:7241/api");
        private readonly HttpClient _client;
        public DashboardController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }
        public JsonResult GetContact(DataTableParams dataTableParams, string? companyName, string? contactName, string? email, string? city)
        {
            int start = dataTableParams.start;
            int length = dataTableParams.length;
            string searchText = dataTableParams.searchText;
            string sortColumn = dataTableParams.sortColumn;
            string sortDirection = dataTableParams.sortDirection;
            List<ContactViewModel> contactList = new List<ContactViewModel>();
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
    }
}
