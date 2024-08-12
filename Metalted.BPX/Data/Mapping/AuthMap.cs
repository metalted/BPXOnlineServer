using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Metalted.BPX.Data.Mapping;

public partial class AuthMap
    : IEntityTypeConfiguration<Metalted.BPX.Data.Entities.Auth>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Metalted.BPX.Data.Entities.Auth> builder)
    {
        #region Generated Configure
        // table
        builder.ToTable("auth", "public");

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

        builder.Property(t => t.AccessToken)
            .IsRequired()
            .HasColumnName("access_token")
            .HasColumnType("text");

        builder.Property(t => t.AccessTokenExpiry)
            .IsRequired()
            .HasColumnName("access_token_expiry")
            .HasColumnType("numeric");

        builder.Property(t => t.RefreshToken)
            .IsRequired()
            .HasColumnName("refresh_token")
            .HasColumnType("text");

        builder.Property(t => t.RefreshTokenExpiry)
            .IsRequired()
            .HasColumnName("refresh_token_expiry")
            .HasColumnType("numeric");

        builder.Property(t => t.DateCreated)
            .IsRequired()
            .HasColumnName("date_created")
            .HasColumnType("timestamp with time zone");

        builder.Property(t => t.DateUpdated)
            .HasColumnName("date_updated")
            .HasColumnType("timestamp with time zone");

        // relationships
        builder.HasOne(t => t.User)
            .WithMany(t => t.Auths)
            .HasForeignKey(d => d.IdUser)
            .HasConstraintName("auth_user_id_fk");

        #endregion
    }

    #region Generated Constants
    public readonly struct Table
    {
        public const string Schema = "public";
        public const string Name = "auth";
    }

    public readonly struct Columns
    {
        public const string Id = "id";
        public const string IdUser = "id_user";
        public const string AccessToken = "access_token";
        public const string AccessTokenExpiry = "access_token_expiry";
        public const string RefreshToken = "refresh_token";
        public const string RefreshTokenExpiry = "refresh_token_expiry";
        public const string DateCreated = "date_created";
        public const string DateUpdated = "date_updated";
    }
    #endregion
}
