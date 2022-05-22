using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder
                .HasKey(appointment => appointment.Id);

            builder
                .Property(appointment => appointment.Disease)
                .HasMaxLength(100);

            builder.ToTable("Appointments");
        }
    }
}
