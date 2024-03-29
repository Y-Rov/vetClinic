﻿using Core.Entities;
using Core.Extensions;
using Core.Interfaces.Repositories;
using Core.Paginator;
using Core.Paginator.Parameters;
using DataAccess.Context;

namespace DataAccess.Repositories
{
    public class AnimalRepository : Repository<Animal>, IAnimalRepository
    {
        private readonly ClinicContext _clinicContext;

        public AnimalRepository(ClinicContext clinicContext) : base(clinicContext) 
        {
            _clinicContext = clinicContext;
        }
        public async Task<PagedList<Appointment>> GetAllAppointmentsWithAnimalIdAsync(AnimalParameters animalParameters)
        {
            var appointments = GetAppointments(_clinicContext.Appointments, animalParameters.animalId);

            PagedList<Appointment> pagedResult;
            if (animalParameters.PageSize == 0)
            {
                pagedResult = await appointments.ToPagedListAsync(animalParameters.PageNumber, appointments.Count());
            }
            else
            {
                pagedResult = await appointments.ToPagedListAsync(animalParameters.PageNumber, animalParameters.PageSize);
            }

            return pagedResult;
        }
        private IQueryable<Appointment> GetAppointments(IQueryable<Appointment> appointments, int animalId)
        {
            if(appointments.Any())
            {
                return appointments.Where(appointment => appointment.AnimalId == animalId);
            }
            return appointments;
        }
    }
}
