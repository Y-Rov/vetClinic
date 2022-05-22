using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class AppointmentUserConfiguration : IEntityTypeConfiguration<AppointmentUser>
    {
        public void Configure(EntityTypeBuilder<AppointmentUser> builder)
        {
            builder.HasKey(ap =>
            new
            {
                ap.UserId,
                ap.AppointmentId
            });
            
            builder
                  .HasOne<Appointment>(appointment => appointment.Appointment)
                    .WithMany(appointmentUser => appointmentUser.AppointmentUsers)
                    .HasForeignKey(appointment => appointment.AppointmentId);

            builder
                .HasOne<User>(user => user.User)
                  .WithMany(appointmentUser => appointmentUser.AppointmentUsers)
                  .HasForeignKey(user => user.UserId);

            builder.ToTable("AppointmentUsers");
        }
    }
}
