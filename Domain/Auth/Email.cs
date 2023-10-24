using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Auth
{
    public class Email
    {
        public string Id { get; private set; }
        public string Value { get; private set; }

        private Email() { }
        public Email(string id, string value) {
            Id = id;
            Value = value;
        }
    }
}
