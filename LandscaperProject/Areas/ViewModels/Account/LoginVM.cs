using System.ComponentModel.DataAnnotations;

namespace LandscaperProject.Areas.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string UsernameOrEmail { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(18)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemembered { get; set; }

    }
}
