using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class SalaryConfiguration : IEntityTypeConfiguration<Salary>
    {
        public void Configure(EntityTypeBuilder<Salary> builder)
        {
            builder.
                HasKey(salary => salary.EmployeeId);

            builder
                .Property(salary => salary.Value)
                .HasColumnType("decimal")
                .HasPrecision(7,2)
                .IsRequired();

            builder.
                HasOne<User>(salary => salary.Employee)
                .WithOne(user => user.Salary)
                .HasForeignKey<Salary>(salary => salary.EmployeeId);

            builder.ToTable("Salaries");
        }
    }
}
