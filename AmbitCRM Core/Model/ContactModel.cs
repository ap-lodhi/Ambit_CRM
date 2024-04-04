using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbitCRM_Core.Model
{
    public class ContactModel
    {
    public int ContactId { get; set; }
    public string? CompanyName { get; set; }
    public string? ContactName { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? Source {  get; set; }
        public string? ContactType { get; set; }
        public string? RM { get; set; }
        public bool IsBookMarked { get; set; }
        public DateTime CreationDate { get; set;}
        public DateTime LastInsertionDate { get; set;}
        public string? LastInsertionWith { get; set; }   
        public Boolean IsActive { get; set; }
        public int? total_records { get; set; }


    }
}
