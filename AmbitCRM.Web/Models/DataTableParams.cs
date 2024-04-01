namespace AmbitCRM.Web.Models
{
    public class DataTableParams
    {
        public string searchText { get; set; }
      //  public int? filterStatus { get; set; }
        public string sortColumn { get; set; }
        public string sortDirection { get; set; }
        public int start { get; set; }
        public int length { get; set; }
    }
}
