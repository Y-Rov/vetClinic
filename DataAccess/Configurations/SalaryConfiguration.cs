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
                HasKey(salary=> salary.Id);

            builder
                .Property(salary => salary.Value)
                .HasColumnType("decimal")
                .HasPrecision(10,2)
                .IsRequired();

            builder
                .Property(salary => salary.Date)
                .HasColumnType("DATETIME")
                .IsRequired();

            builder.
                HasOne<User>(salary => salary.Employee)
                .WithMany(user => user.Salaries)
                .HasForeignKey(salary => salary.EmployeeId);

            builder.ToTable("Salaries");
        }
    }
}
