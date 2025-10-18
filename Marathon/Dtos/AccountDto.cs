namespace Marathon.Dtos
{
    public class AccountDto
    {
        public class LoginAccountDto
        {
            public string Email { get; set; } = null!;

            public string PasswordHash { get; set; } = null!;
        }

        public class TokenRequestDto
        {
            public string? AccessToken { get; set; }
            public string? RefreshToken { get; set; }
        }
    }
}
