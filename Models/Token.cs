namespace project_backend.Models
{
    public class Token : Response
    {
        public string token { get; }

        public Token(string tok)
        {
            token = tok;
        }
    }
}
