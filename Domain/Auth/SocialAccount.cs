using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Auth
{
    public class SocialAccount
    {
        public string Id { get; set; }
        public string Value { get; private set; }

        private SocialAccount() { }
        public SocialAccount(string id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}
