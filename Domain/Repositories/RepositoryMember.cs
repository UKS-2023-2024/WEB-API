using Domain.Auth;
using Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class RepositoryMember
    {
        public Guid Id { get; private set; }
        public User Member { get; private set; }
        public Repository Repository { get; private set; }
        public Guid RepositoryId { get; private set; }

        public RepositoryMember()
        {
        }
    }
}
