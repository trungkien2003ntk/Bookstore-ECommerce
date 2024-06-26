﻿using KKBookstore.Domain.Aggregates.ProductAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KKBookstore.Infrastructure.Data.Configurations;

internal class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable($"{nameof(Rating)}s");

        var converter = new EnumToStringConverter<RatingStatus>();

        builder.Property(t => t.Id)
            .HasColumnName($"{nameof(Rating)}Id");

        builder.Property(t => t.SkuId)
            .IsRequired();

        builder.Property(t => t.RatingValue)
            .IsRequired();

        builder.Property(t => t.Comment)
            .HasColumnType("nvarchar(max)");

        builder.Property(t => t.Response)
            .HasColumnType("nvarchar(max)");
            
        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion(converter);

        builder.HasOne(t => t.Sku)
            .WithMany(t => t.Ratings)
            .HasForeignKey(t => t.SkuId);

        builder.HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId);

        builder.HasOne(t => t.LastEditedByUser)
            .WithMany()
            .HasForeignKey(t => t.LastEditedBy);

        builder.HasOne(t => t.CreatedByUser)
            .WithMany()
            .HasForeignKey(t => t.CreatedBy);
    }
}
