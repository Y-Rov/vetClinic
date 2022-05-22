using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder
                .HasKey(address => address.Id);

            builder
                .Property(address => address.City)
                .HasMaxLength(85)
                .IsRequired();

            builder
                .Property(address => address.Street)
                .HasMaxLength(85)
                .IsRequired();

            builder
                .Property(address => address.House)
                .HasMaxLength(10)
                .IsRequired();

            builder
                .Property(address => address.BuildingNumber)
                .HasColumnType("tinyint");

            builder
                .Property(address => address.ApartmentNumber)
                .HasColumnType("smallint");

            builder
                .Property(address => address.ZipCode)
                .HasColumnType("int");

            builder
                .HasOne<User>(address => address.User)
                .WithOne(user => user.Address)
                .HasForeignKey<Address>(address => address.UserId);

            builder.ToTable("Addresses");
        }
    }
}

