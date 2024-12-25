namespace DogGrommingBackend.DTOModels
{
    public class AppointmentDTO
    {
        public class AppointmentDto
        {
            public int AppointmentId { get; set; }
            public DateTime AppointmentTime { get; set; }
            public int? CustomerId { get; set; }
            public int? BranchId { get; set; }
            public bool IsActive { get; set; }
            public DateTime CreateDate { get; set; }
            public string? FullName { get; set; }
        }

    }
}
