﻿using LibraryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.Infrastructure.Persistance.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Id)
                .ValueGeneratedOnAdd();

            builder.Property(b => b.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(b => b.Authors)
                .WithMany(a => a.Books);
        }
    }
}
