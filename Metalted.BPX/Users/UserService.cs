using System.Diagnostics.CodeAnalysis;
using Metalted.BPX.Data.Entities;

namespace Metalted.BPX.Users;

public interface IUserService
{
    User Create(ulong steamId,string steamName);
    bool TryGet(int userId, [NotNullWhen(true)] out User? user);
    bool TryGet(ulong steamId, [NotNullWhen(true)] out User? user);
}

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public User Create(ulong steamId, string steamName)
    {
        User user = new()
        {
            SteamId = steamId,
            SteamName = steamName,
            Banned = false
        };

        return _repository.Insert(user);
    }

    public bool TryGet(int userId, [NotNullWhen(true)] out User? user)
    {
        user = _repository.GetById(userId);
        return user != null;
    }

    public bool TryGet(ulong steamId, [NotNullWhen(true)] out User? user)
    {
        user = _repository.GetSingle(x => x.SteamId == steamId);
        return user != null;
    }
}
