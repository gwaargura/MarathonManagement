namespace MM_client.Models
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}
