using DogGrommingBackend.Data;
using DogGrommingBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using static DogGrommingBackend.DTOModels.AppointmentDTO;

namespace DogGrommingBackend.Logic
{
    public class AppointmentLogic
    {
        private readonly EntityDbContext _context;
        private readonly ILogger<AppointmentLogic> _logger;

        public AppointmentLogic(EntityDbContext context, ILogger<AppointmentLogic> logger)
        {
            _context = context;
            _logger = logger;
        }

        //to do: delete
        //public async Task<List<Appointment>> GetAllAppointmentsAsync()
        //{
        //    try
        //    {
        //        var appointments = await (from appointment in _context.Appointments
        //                                  join customer in _context.Customers
        //                                  on appointment.CustomerId equals customer.CustomerId
        //                                  where appointment.IsActive // Optionally filter for active appointments
        //                                  select new Appointment
        //                                  {
        //                                      AppointmentId = appointment.AppointmentId,
        //                                      AppointmentTime = appointment.AppointmentTime,
        //                                      CustomerId = appointment.CustomerId,
        //                                      BranchId = appointment.BranchId,
        //                                      IsActive = appointment.IsActive,
        //                                      CreateDate = appointment.CreateDate,
        //                                      FullName = customer.FullName // Map FullName to the DTO
        //                                  }).ToListAsync();

        //        return appointments;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while fetching appointments.");
        //        throw;
        //    }
        //}

        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            try
            {
                var appointments = await _context.AppointmentDto
                .FromSqlInterpolated($"EXEC GetAllAppointments")
                .ToListAsync();

                return appointments;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred.");
                throw;
            }
        }



        public async Task AddAppointmentAsync(Appointment newAppointment)
        {
            try
            {
                if (newAppointment == null)
                    throw new ArgumentNullException(nameof(newAppointment));

                var filterAppointment = new Appointment
                {
                    AppointmentTime = newAppointment.AppointmentTime,
                    CustomerId = newAppointment.CustomerId,
                    BranchId = newAppointment.BranchId,
                    IsActive = true,
                    CreateDate = DateTime.Now                    
                };
                                

                _context.Appointments.Add(filterAppointment);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new appointment.");
                throw;
            }
        }

        
        public async Task UpdateAppointmentAsync(int appointmentId, DateTime newAppointmentTime)
        {
            try
            {
                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

                if (appointment == null)
                    throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");

                appointment.AppointmentTime = newAppointmentTime;
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the appointment.");
                throw;
            }
        }

       
        public async Task DeleteAppointmentAsync(int appointmentId)
        {
            try
            {
                var appointment = await _context.Appointments
                    .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

                if (appointment == null)
                    throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");

                appointment.IsActive = false;
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the appointment.");
                throw;
            }
        }
               
    }
}
