namespace AmbitCRM.Web.Models
{
    public class BookmarkedModel
    {
        public int BookMarkedId { get; set; }       
        public string UserId { get; set; }
        public int ContactId { get; set; }
        public bool IsBookMarked { get; set; }
    }
}
