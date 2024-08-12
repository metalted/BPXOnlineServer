using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Metalted.BPX.Data;

public partial class BpxContext : DbContext
{
    public BpxContext(DbContextOptions<BpxContext> options)
        : base(options)
    {
    }

    #region Generated Properties
    public virtual DbSet<Metalted.BPX.Data.Entities.Auth> Auths { get; set; } = null!;

    public virtual DbSet<Metalted.BPX.Data.Entities.Blueprint> Blueprints { get; set; } = null!;

    public virtual DbSet<Metalted.BPX.Data.Entities.User> Users { get; set; } = null!;

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Generated Configuration
        modelBuilder.ApplyConfiguration(new Metalted.BPX.Data.Mapping.AuthMap());
        modelBuilder.ApplyConfiguration(new Metalted.BPX.Data.Mapping.BlueprintMap());
        modelBuilder.ApplyConfiguration(new Metalted.BPX.Data.Mapping.UserMap());
        #endregion
    }
}
