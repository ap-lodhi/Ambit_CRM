using AmbitCRM_Core.Interface;
using AmbitCRM_Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;

namespace Ambit_CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IDALContact _IContact;
        private readonly IConfiguration _Config;
        public ContactController(IDALContact IContact, IConfiguration config)
        {
            _IContact = IContact;
            _Config = config;
        }


        [HttpGet]
        [Route("ContactList")]
        public ActionResult ContactList(int? start, int? length, string? searchText, string? sortColumn, string? sortDirection, string? CompanyName, string? ContactName, string? Email, string? City ,string? SearchType)
        {
            List<ContactModel> contactList = new List<ContactModel>();
            contactList = _IContact.GetContactList(start, length, searchText, sortColumn, sortDirection, CompanyName, ContactName, Email, City, SearchType);
            if (contactList.Count == 0)
            {
                return NotFound("List not  Found");
            }
            return Ok(contactList);
        }

        [HttpGet]
        [Route("GetBoomarkedList")]
        public ActionResult GetBoomarkedList(string? id, int? start, int? length, string? sortColumn, string? sortDirection)
        {
            List<ContactModel> BookMarked = new List<ContactModel>();
            BookMarked = _IContact.GetBookMakedList(id,start, length, sortColumn, sortDirection);
            if (BookMarked.Count == 0)
            {
                return NotFound("List not  Found");
            }
            return Ok(BookMarked);
        }



        [HttpPost, Route("SaveBookmarked")]
        public ActionResult SaveBookmarked(BookmarkedModel BM)
        {
          
           var   res =  _IContact.BookmarkStatus(BM);

            return Ok(res);
        }


        [HttpPost, Route("SaveSearch")]
        public ActionResult SaveSearch(SearchModel SM)

        {

            var res = _IContact.SaveSearch(SM);

            return Ok(res);
        }

        [HttpGet]
        [Route("GetSearchDetails")]

        public ActionResult GetSearchDetails(string id)
        {
            List<SearchModel> searchList = new List<SearchModel>();
            searchList = _IContact.GetSearchList(id);
            
            return Ok(searchList);  


        }

        [HttpDelete]
        [Route("DeleteSearch")]

        public ActionResult DeleteSearch(int id)
        {
         ResponseModel res  = new ResponseModel();
            res = _IContact.DeletePreviousSearch(id);

            return Ok(res);


        }

    }
}
