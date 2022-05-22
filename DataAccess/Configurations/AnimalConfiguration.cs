using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
                .HasForeignKey(animal => animal.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Animals");
        }
    }
}
