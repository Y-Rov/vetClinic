using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class ExceptionConfigurations : IEntityTypeConfiguration<ExceptionEntity>
    {
        public void Configure(EntityTypeBuilder<ExceptionEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("Exceptions");
        }
    }
}
