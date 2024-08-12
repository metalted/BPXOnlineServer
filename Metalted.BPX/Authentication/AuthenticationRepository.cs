using Metalted.BPX.Data.Entities;
using Metalted.BPX.DataStore;

namespace Metalted.BPX.Authentication;

public interface IAuthenticationRepository : IBasicRepository<Auth>
{
}

public class AuthenticationRepository : BasicRepository<Auth>, IAuthenticationRepository
{
    public AuthenticationRepository(IDatabase database, ILogger<AuthenticationRepository> logger)
        : base(database, logger)
    {
    }
}
