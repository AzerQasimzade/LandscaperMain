using System.ComponentModel.DataAnnotations;

namespace LandscaperProject.Areas.ViewModels.Service
{
    public class CreateServiceVM
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
