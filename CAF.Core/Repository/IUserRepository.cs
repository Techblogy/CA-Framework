using CAF.Core.Entities;
using CAF.Core.Repository.Base;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Data;
using System.Linq;

namespace CAF.Core.Repository
{
    public interface IUserRepository : IRepository<User, long>
    {
        User Login(string email, string password);
        IQueryable<User> GetAllUsersWithJoins();

        DataTable GetQuery(string query);

        bool DatabaseInitializer(IConfiguration configuration, IServiceCollection services);
    }
}
