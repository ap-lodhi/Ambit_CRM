namespace AmbitCRM.Web.Models
{
    public class ResponseModel
    {

        public bool status { get; set; }
        public string? message { get; set; }
        public string? auth_token { get; set; }
        public string? Empcode { get; set; }
    }
}
