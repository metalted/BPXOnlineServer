using FluentResults;
using FluentStorage;
using FluentStorage.Blobs;
using Microsoft.Extensions.Options;

namespace Metalted.BPX.Storage;

public interface IStorageService
{
    Task<Result<bool>> Exists(int userId, string name);
    Task<Result<byte[]>> DownloadBlueprint(int userId, string name);
    Task<Result<byte[]>> DownloadImage(int userId, string name);
    Task<Result> SaveBlueprint(int userId, string name, byte[] buffer);
    Task<Result> SaveImage(int userId, string name, byte[] buffer);
}

public class StorageService : IStorageService
{
    private readonly ILogger<StorageService> _logger;
    private readonly IBlobStorage _storage;

    public StorageService(ILogger<StorageService> logger, IOptions<StorageOptions> options)
    {
        _logger = logger;
        _storage = StorageFactory.Blobs.DirectoryFiles(options.Value.Path);
    }

    public async Task<Result<bool>> Exists(int userId, string name)
    {
        string path = StoragePath.Combine(userId.ToString(), name + ".zeeplevel");
        try
        {
            return await _storage.ExistsAsync(path);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to check if blueprint exists");
            return Result.Fail("Failed to check if blueprint exists");
        }
    }

    public async Task<Result<byte[]>> DownloadBlueprint(int userId, string name)
    {
        try
        {
            string path = StoragePath.Combine(userId.ToString(), name + ".zeeplevel");
            await using Stream stream = await _storage.OpenReadAsync(path);
            using BinaryReader reader = new(stream);
            return reader.ReadBytes((int)stream.Length);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to download blueprint");
            return Result.Fail("Failed to download blueprint");
        }
    }

    public async Task<Result<byte[]>> DownloadImage(int userId, string name)
    {
        try
        {
            string path = StoragePath.Combine(userId.ToString(), name + ".png");
            await using Stream stream = await _storage.OpenReadAsync(path);
            using BinaryReader reader = new(stream);
            return reader.ReadBytes((int)stream.Length);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "failed to download blueprint");
            return Result.Fail("Failed to download blueprint");
        }
    }

    public async Task<Result> SaveBlueprint(int userId, string name, byte[] buffer)
    {
        string path = StoragePath.Combine(userId.ToString(), name + ".zeeplevel");
        try
        {
            await _storage.WriteAsync(path, buffer);
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save blueprint");
            return Result.Fail("Failed to save blueprint");
        }
    }

    public async Task<Result> SaveImage(int userId, string name, byte[] buffer)
    {
        string path = StoragePath.Combine(userId.ToString(), name + ".png");
        try
        {
            await _storage.WriteAsync(path, buffer);
            return Result.Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save blueprint");
            return Result.Fail("Failed to save blueprint");
        }
    }
}
