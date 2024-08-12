using Metalted.BPX.Data.Entities;
using Metalted.BPX.DataStore;

namespace Metalted.BPX.Blueprints;

public interface IBlueprintRepository : IBasicRepository<Blueprint>
{
}

public class BlueprintRepository : BasicRepository<Blueprint>, IBlueprintRepository
{
    public BlueprintRepository(IDatabase database, ILogger<BlueprintRepository> logger) 
        : base(database, logger)
    {
    }
}
