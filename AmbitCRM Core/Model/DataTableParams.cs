using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbitCRM_Core.Model
{
    public class DataTableParams
    {
        public string? searchText { get; set; }
   
        public string? sortColumn { get; set; }
        public string? sortDirection { get; set; }
        public int? start { get; set; }
        public int? length { get; set; }
    }
}
