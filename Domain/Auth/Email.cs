using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Auth
{
    public class Email
    {
        public string Value { get; private set; }
        public User? User { get; private set; } = null;
        public Guid UserId { get; private set; }

        private Email() { }
        public Email(string value) {
            Value = value;
        }
    }
}
