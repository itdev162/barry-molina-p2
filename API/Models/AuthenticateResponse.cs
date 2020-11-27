
namespace API.Models
{
    public class AuthenticateResponse
    {
        public string token { get; set; }

        public AuthenticateResponse(string t)
        {
            token = t;
        }

    }
}