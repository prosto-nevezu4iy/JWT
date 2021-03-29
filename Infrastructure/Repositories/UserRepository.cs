using Application.Common.Interfaces;
using System.Data;

namespace Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IDbTransaction transaction)
            : base(transaction)
        {
        }
    }
}
