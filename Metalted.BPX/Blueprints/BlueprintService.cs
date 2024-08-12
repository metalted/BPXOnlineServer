using FluentResults;
using Metalted.BPX.Blueprints.Resources;
using Metalted.BPX.Data.Entities;
using Metalted.BPX.Storage;
using Metalted.BPX.Users;
using Microsoft.EntityFrameworkCore;

namespace Metalted.BPX.Blueprints;

public interface IBlueprintService
{
    Result<bool> Exists(ulong steamId, string name);
    IEnumerable<Blueprint> List(int page, int pageSize);
    Task<Result<Blueprint>> Submit(ulong steamId, BlueprintResource resource);
    IEnumerable<Blueprint> Search(SearchResource resource);
}

public class BlueprintService : IBlueprintService
{
    private readonly IBlueprintRepository _repository;
    private readonly IStorageService _storageService;
    private readonly IUserService _userService;

    public BlueprintService(
        IBlueprintRepository repository,
        IStorageService storageService,
        IUserService userService)
    {
        _repository = repository;
        _storageService = storageService;
        _userService = userService;
    }

    public Result<bool> Exists(ulong steamId, string name)
    {
        if (!_userService.TryGet(steamId, out User? user))
        {
            return Result.Fail("User not found");
        }

        if (user.Banned)
        {
            return Result.Fail("User is banned");
        }

        return _repository.GetSingle(x => x.Name == name && x.IdUser == user.Id) != null;
    }

    public IEnumerable<Blueprint> List(int page, int pageSize)
    {
        return _repository.GetAll().Skip(page * pageSize).Take(pageSize);
    }

    public async Task<Result<Blueprint>> Submit(ulong steamId, BlueprintResource resource)
    {
        if (!_userService.TryGet(steamId, out User? user))
        {
            return Result.Fail("User not found");
        }

        if (user.Banned)
        {
            return Result.Fail("User is banned");
        }

        Blueprint? existing = _repository.GetSingle(x=>x.Name == resource.Name && x.IdUser == user.Id);

        if (existing != null)
        {
            Result saveResult = await UploadData(user, resource, existing.FileId);
            if (saveResult.IsFailed)
            {
                return saveResult;
            }

            if (resource.Tags != null && resource.Tags.Count > 0)
            {
                existing.Tags = resource.Tags;
            }

            existing = _repository.Update(existing);
            return Result.Ok(existing);
        }
        else
        {
            string fileGuid = Guid.NewGuid().ToString();
            Result saveResult = await UploadData(user, resource, fileGuid);
            if (saveResult.IsFailed)
            {
                return saveResult;
            }

            Blueprint blueprint = _repository.Insert(
                new Blueprint
                {
                    IdUser = user.Id,
                    Name = resource.Name,
                    FileId = fileGuid,
                    Tags = resource.Tags
                });

            return Result.Ok(blueprint);
        }
    }

    private async Task<Result> UploadData(User user, BlueprintResource resource, string fileGuid)
    {
        Result saveResult = await _storageService.SaveBlueprint(
            user.Id,
            fileGuid,
            Convert.FromBase64String(resource.BlueprintBase64));

        if (saveResult.IsFailed)
        {
            return saveResult;
        }

        Result saveImageResult = await _storageService.SaveImage(
            user.Id,
            fileGuid,
            Convert.FromBase64String(resource.ImageBase64));

        if (saveImageResult.IsFailed)
        {
            return saveImageResult;
        }
        
        return Result.Ok();
    }


    public IEnumerable<Blueprint> Search(SearchResource resource)
    {
        List<Blueprint> blueprints = _repository.GetAll(set => set.Include(x => x.User)).ToList();
        List<Blueprint> filtered = new();

        foreach (Blueprint blueprint in blueprints)
        {
            if (!string.IsNullOrWhiteSpace(resource.Creator) && blueprint.User.SteamName.Contains(
                    resource.Creator,
                    StringComparison.OrdinalIgnoreCase))
            {
                filtered.Add(blueprint);
            }
            else if (MatchesAllTerms(blueprint, resource.Tags))
            {
                filtered.Add(blueprint);
            }
            else if (MatchesAllTags(blueprint, resource.Tags))
            {
                filtered.Add(blueprint);
            }
        }

        return filtered;
    }
    
    private static bool MatchesAllTerms(Blueprint blueprint, string[] terms)
    {
        return terms.Length != 0 && terms.All(term => blueprint.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
    }
    
    private static bool MatchesAllTags(Blueprint blueprint, string[] tags)
    {
        return tags.Length != 0 && tags.All(tag => blueprint.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase));
    }
}
