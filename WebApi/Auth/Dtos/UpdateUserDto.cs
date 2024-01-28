using Domain.Auth;

namespace WEB_API.Auth.Dtos
{
    public class UpdateUserDto
    {
        public string? FullName { get; set; }

        public string? Bio { get; set; }

        public string? Company { get; set; }

        public string? Location { get; set; }
        public string? Website { get; set; }
        public List<SocialAccountDto>? SocialAccounts { get; set; } 

    }
}
