using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ConsumeApiTask1.Models
{
    public class Customer
    {
        [Display(Name = "CustomerId")]
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone_Number { get; set; }
        public string Country_code { get; set; }
        [Required(ErrorMessage ="Please select one")]
        public string Gender { get; set; }
        public decimal Balance { get; set; }
    }
}
