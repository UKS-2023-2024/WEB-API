using Domain.Auth;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WEB_API.Auth.Dtos
{
    public class SocialAccountDto
    {
        public Guid? Id { get; set; }
        public string Value { get; set; }

        public SocialAccountDto(string value)
        {
            Value = value;
        }
        private SocialAccountDto(Guid?id, string value)
        {
            Id = id;
            Value = value;
        }

        public static List<SocialAccount> SocialAccountsFromSocialAccountDtos(List<SocialAccountDto>? dtos, Guid userId)
        {
            if (dtos == null) dtos = new();
            List<SocialAccount> retList = new();
            foreach (SocialAccountDto dto in dtos)
            {
                retList.Add(SocialAccount.Create(dto.Id, dto.Value, userId));
            }
            return retList;
        }

        public static List<SocialAccountDto> SocialAccountDtosFromSocialAccounts(List<SocialAccount> accounts)
        {
            List<SocialAccountDto> retList = new();
            foreach (SocialAccount acc in accounts)
            {
                retList.Add(new SocialAccountDto(acc.Id, acc.Value));
            }
            return retList;
        }
    }
}
