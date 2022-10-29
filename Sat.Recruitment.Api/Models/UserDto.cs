using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Api.Models
{
    public partial class UserDto
    {
        [Required(ErrorMessage = "The name is required", AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Required(ErrorMessage = "The email is required", AllowEmptyStrings = false)]
        public string Email { get; set; }
        [Required(ErrorMessage = "The address is required", AllowEmptyStrings = false)]
        public string Address { get; set; }
        [Required(ErrorMessage = "The phone is required", AllowEmptyStrings = false)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "The userType is required", AllowEmptyStrings = false)]
        public string UserType { get; set; }
        public decimal Money { get; set; }
    }
}