
using System.ComponentModel.DataAnnotations;

namespace LandscaperProject.Areas.ViewModels.Account
{
    public class RegisterVM
    {
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Username { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(315)]
        public string Name  { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Surname { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(18)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


    }
}
