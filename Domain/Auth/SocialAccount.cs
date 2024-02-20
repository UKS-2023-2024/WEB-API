using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Auth
{
    public class SocialAccount
    {
        public Guid? Id { get; private set; }
        public string Value { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        private SocialAccount() { }
        private SocialAccount(Guid? id, string value, Guid userId)
        {
            Value = value;
            UserId = userId;
        }
        public static SocialAccount Create(Guid? id, string value, Guid userId)
        {
            return new SocialAccount(id, value, userId);
        }
    }
}
