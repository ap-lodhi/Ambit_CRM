using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbitCRM_Core.Model
{
    public class SearchModel
    {
      public int SearchId { get; set; } 
        public string? SearchType { get; set; }
        public string? UserID { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactName { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }

    }
}
