using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Metalted.BPX.Data.Mapping;

public partial class UserMap
    : IEntityTypeConfiguration<Metalted.BPX.Data.Entities.User>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Metalted.BPX.Data.Entities.User> builder)
    {
        #region Generated Configure
        // table
        builder.ToTable("user", "public");

        // key
        builder.HasKey(t => t.Id);

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName("id")
            .HasColumnType("integer")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.SteamId)
            .IsRequired()
            .HasColumnName("steam_id")
            .HasColumnType("numeric");

        builder.Property(t => t.SteamName)
            .IsRequired()
            .HasColumnName("steam_name")
            .HasColumnType("character varying(32)")
            .HasMaxLength(32);

        builder.Property(t => t.Banned)
            .IsRequired()
            .HasColumnName("banned")
            .HasColumnType("boolean");

        builder.Property(t => t.DateCreated)
            .IsRequired()
            .HasColumnName("date_created")
            .HasColumnType("timestamp with time zone");

        builder.Property(t => t.DateUpdated)
            .HasColumnName("date_updated")
            .HasColumnType("timestamp with time zone");

        // relationships
        #endregion
    }

    #region Generated Constants
    public readonly struct Table
    {
        public const string Schema = "public";
        public const string Name = "user";
    }

    public readonly struct Columns
    {
        public const string Id = "id";
        public const string SteamId = "steam_id";
        public const string SteamName = "steam_name";
        public const string Banned = "banned";
        public const string DateCreated = "date_created";
        public const string DateUpdated = "date_updated";
    }
    #endregion
}
