namespace project_backend.Models
{
    public class Error : Response
    {
        public string message { get; set; }
        public Error(string errMessage)
        {
            message = errMessage;
        }
    }
}
