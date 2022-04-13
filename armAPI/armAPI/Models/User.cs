using System.ComponentModel.DataAnnotations;

namespace armAPI
{
public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; } = string.Empty;
        [Required]
        public string Surname { get; set; } =  string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string Phonenumber { get; set; } = string.Empty;
        [Required]
        public int Role { get; set; } 
        [Required]
        public int Subscription { get; set; }
        
        public string Postcode { get; set; } = string.Empty;
        [Required] 
        public string Dob { get; set; } = string.Empty;
    }


}
