namespace AmbitCRM.Web.Models
{
    public class ContactViewModel
    {
        public int ContactId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Source { get; set; }
        public string ContactType { get; set; }
        public string RM { get; set; }
      

        public DateTime CreationDate { get; set; }
        public DateTime LastInsertionDate { get; set; }
        public string? LastInsertionWith { get; set; }
        public Boolean IsActive { get; set; }
        public int total_records { get; set; }
    }
}
