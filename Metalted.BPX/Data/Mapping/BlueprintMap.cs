using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Metalted.BPX.Data.Mapping;

public partial class BlueprintMap
    : IEntityTypeConfiguration<Metalted.BPX.Data.Entities.Blueprint>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Metalted.BPX.Data.Entities.Blueprint> builder)
    {
        #region Generated Configure
        // table
        builder.ToTable("blueprint", "public");

        // key
        builder.HasKey(t => t.Id);

        // properties
        builder.Property(t => t.Id)
            .IsRequired()
            .HasColumnName("id")
            .HasColumnType("integer")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.IdUser)
            .IsRequired()
            .HasColumnName("id_user")
            .HasColumnType("integer");

        builder.Property(t => t.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasColumnType("text");

        builder.Property(t => t.Tags)
            .IsRequired()
            .HasColumnName("tags")
            .HasColumnType("text[]");

        builder.Property(t => t.FileId)
            .IsRequired()
            .HasColumnName("file_id")
            .HasColumnType("character varying(36)")
            .HasMaxLength(36);

        builder.Property(t => t.DateCreated)
            .IsRequired()
            .HasColumnName("date_created")
            .HasColumnType("timestamp with time zone");

        builder.Property(t => t.DateUpdated)
            .HasColumnName("date_updated")
            .HasColumnType("timestamp with time zone");

        // relationships
        builder.HasOne(t => t.User)
            .WithMany(t => t.Blueprints)
            .HasForeignKey(d => d.IdUser)
            .HasConstraintName("blueprint_user_id_fk");

        #endregion
    }

    #region Generated Constants
    public readonly struct Table
    {
        public const string Schema = "public";
        public const string Name = "blueprint";
    }

    public readonly struct Columns
    {
        public const string Id = "id";
        public const string IdUser = "id_user";
        public const string Name = "name";
        public const string Tags = "tags";
        public const string FileId = "file_id";
        public const string DateCreated = "date_created";
        public const string DateUpdated = "date_updated";
    }
    #endregion
}
