﻿using AmbitCRM_Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbitCRM_Core.Interface
{
    public interface IDALContact
    {
        List<ContactModel> GetContactList(int? start, int? length, string? searchText, string? sortColumn, string? sortDirection, string? CompanyName, string? ContactName, string? Email, string? City);

        ResponseModel BookmarkStatus(BookmarkedModel BM);

    }
}