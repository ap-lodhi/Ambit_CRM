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
        public string CreatedDate { get; set;}
        public string LastInteractionDate { get; set;}
        public string? LastInteractionWith { get; set; }   
        public Boolean IsActive { get; set; }
        public int? total_records { get; set; }


    }
}
