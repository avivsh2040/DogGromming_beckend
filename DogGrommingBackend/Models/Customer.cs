using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DogGrommingBackend.Models
{
    public class Customer
    {
        [Key]    
        public int CustomerId { get; set; } 
        public string ?FullName { get; set; }
        public string ?UserName { get; set; }
        public string ?Password { get; set; } // encrypted password
        public DateTime ?CreateDate { get; set; }
        public int BranchId { get; set; }  

    }
}
