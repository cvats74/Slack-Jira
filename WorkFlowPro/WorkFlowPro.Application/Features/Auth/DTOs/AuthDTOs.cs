using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowPro.Application.Features.Auth.DTOs
{
    public class RegisterDto
    {
        [Required (ErrorMessage ="First Name Required")]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name Required")]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = string.Empty;

        // Plain text password from user
        // We hash this before storing
        [Required(ErrorMessage = "Password is  Required")]
        [MinLength(8, ErrorMessage = " Password should be at least 8 characters")]
        public string Password { get; set; } = string.Empty;

        // Name of company being created
        [Required(ErrorMessage = "Organisation name is  Required")]
        public string OrganizationName { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is  Required")]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public DateTime TokenExpiry { get; set; }
    }
}
