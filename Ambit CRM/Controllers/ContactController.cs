﻿using AmbitCRM_Core.Interface;
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
        public ActionResult ContactList(int? start, int? length, string? searchText, string? sortColumn, string? sortDirection, string? CompanyName, string? ContactName, string? Email, string? City)
        {
            List<ContactModel> contactList = new List<ContactModel>();
            contactList = _IContact.GetContactList(start, length, searchText, sortColumn, sortDirection, CompanyName, ContactName, Email, City);
            if (contactList.Count == 0)
            {
                return NotFound("List not  Found");
            }
            return Ok(contactList);
        }

        [HttpPost, Route("BookmarkedStatus")]
        public ActionResult BookmarkedStatus(BookmarkedModel BM)
        {
          
           var   res =  _IContact.BookmarkStatus(BM);

            return Ok(res);
        }
       

    }
}