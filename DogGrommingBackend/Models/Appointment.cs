using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DogGrommingBackend.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; } 
        public DateTime AppointmentTime { get; set; }
        public int ?CustomerId { get; set; }
        public int ?BranchId { get; set; } 
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        //[NotMapped]
        //public string? FullName { get; set; }

    }
}
