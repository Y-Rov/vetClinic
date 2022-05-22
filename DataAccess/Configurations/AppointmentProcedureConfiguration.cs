using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class AppointmentProcedureConfiguration : IEntityTypeConfiguration<AppointmentProcedure>
    {
        public void Configure(EntityTypeBuilder<AppointmentProcedure> builder)
        {
            builder.HasKey(ap =>
            new 
            { 
                ap.ProcedureId, 
                ap.AppointmentId 
            });

            builder
                  .HasOne<Appointment>(appointment => appointment.Appointment)
                    .WithMany(appointmentProcedure => appointmentProcedure.AppointmentProcedures)
                    .HasForeignKey(appointment => appointment.AppointmentId);

            builder
                 .HasOne<Procedure>(procedure => procedure.Procedure)
                   .WithMany(appointmentProcedure => appointmentProcedure.AppointmentProcedures)
                   .HasForeignKey(procedure => procedure.ProcedureId);

            builder.ToTable("AppointmentProcedures");
        }
    }
}
