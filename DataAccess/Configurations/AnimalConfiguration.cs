using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    public class AnimalConfiguration : IEntityTypeConfiguration<Animal>
    {
        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder
                .HasKey(animal => animal.Id);

            builder
                .Property(animal => animal.NickName)
                .HasMaxLength(50);

            builder
                .HasOne<User>(animal => animal.Owner)
                .WithMany(user => user.Animals)
                .HasForeignKey(animal => animal.OwnerId);

            builder.ToTable("Animals");
        }
    }
}
