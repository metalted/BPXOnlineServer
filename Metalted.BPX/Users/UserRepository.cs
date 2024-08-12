using Metalted.BPX.Data.Entities;
using Metalted.BPX.DataStore;

namespace Metalted.BPX.Users;

public interface IUserRepository : IBasicRepository<User>
{
}

public class UserRepository : BasicRepository<User>, IUserRepository
{
    public UserRepository(IDatabase database, ILogger<UserRepository> logger)
        : base(database, logger)
    {
    }
}
