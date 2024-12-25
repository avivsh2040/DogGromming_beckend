using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using DogGrommingBackend.Models;
using DogGrommingBackend.Logic;
using Microsoft.EntityFrameworkCore;

namespace DogGrommingBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentLogic _appointmentLogic;

        public AppointmentController(AppointmentLogic appointmentLogic)
        {
            _appointmentLogic = appointmentLogic;
        }

       
        [HttpGet("GetAppointments")]
        public async Task<IActionResult> GetAppointments()
        {
            var appointments = await _appointmentLogic.GetAllAppointmentsAsync();
            return Ok(appointments);
        }

        
        [HttpPut("AddAppointment")]
        public async Task<IActionResult> AddAppointment([FromBody] Appointment appointment)
        {
            DateTime utcAppointmentTime = appointment.AppointmentTime.ToLocalTime();

            if (appointment == null)
            {
                return BadRequest("Appointment data is required.");
            }

            appointment.BranchId = 1;
            appointment.CreateDate = DateTime.Now;
            appointment.IsActive = true;
            appointment.AppointmentTime = utcAppointmentTime;
            await _appointmentLogic.AddAppointmentAsync(appointment);
            
            return CreatedAtAction(nameof(GetAppointments), new { id = appointment.AppointmentId }, appointment);
        }


       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {

            try
            {
                await _appointmentLogic.DeleteAppointmentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAppointment([FromBody] Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest("Appointment data is required.");
            }

            try
            {
                await _appointmentLogic.UpdateAppointmentAsync(appointment.AppointmentId, appointment.AppointmentTime);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        
        }
    }
}
