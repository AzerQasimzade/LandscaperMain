using System.ComponentModel.DataAnnotations;

namespace LandscaperProject.Areas.ViewModels.Service
{
    public class UpdateServiceVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Photo { get; set; }
        public string Image { get; set; }
    }
}
