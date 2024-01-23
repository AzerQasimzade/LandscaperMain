using LandscaperProject.Areas.ViewModels.Service;
using LandscaperProject.DAL;
using LandscaperProject.Models;
using LandscaperProject.Utilities.Enums;
using LandscaperProject.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LandscaperProject.Areas.LandscaperAdmin.Controllers
{
    [Area("LandscaperAdmin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Service> services = await _context.Services.ToListAsync();
            return View(services);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVM serviceVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!serviceVM.Photo.ValidateFileType(FileHelper.Image))
            {
                ModelState.AddModelError("Photo", "Photo type is incorrect");
                return View();
            }
            if (!serviceVM.Photo.ValidateFileSize(SizeHelper.gb))
            {
                ModelState.AddModelError("Photo", "Photo size is incorrect");
                return View();
            }
            string filename=Guid.NewGuid().ToString()+serviceVM.Photo.FileName;
            string path = Path.Combine(_env.WebRootPath, "assets", "Landscaper", "img", "services", filename);
            FileStream fileStream = new FileStream(path, FileMode.Create);
            await serviceVM.Photo.CopyToAsync(fileStream);
            Service service = new Service
            {
                Image=filename,
                Title = serviceVM.Title,
                Description = serviceVM.Description,
            };
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id<=0)
            {
                return BadRequest();
            }
            Service existed = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (existed == null)
            {
                return NotFound();
            }
            UpdateServiceVM serviceVM = new UpdateServiceVM
            {
                Description = existed.Description,
                Image = existed.Image,
                Title = existed.Title,
            };
            return View(serviceVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateServiceVM serviceVM)
        {
            if (!ModelState.IsValid)
            {
                return View(serviceVM);
            }
            Service existed = _context.Services.FirstOrDefault(x => x.Id == id);
            if (serviceVM.Photo is not null)
            {
                if (!serviceVM.Photo.ValidateFileType(FileHelper.Image))
                {
                    ModelState.AddModelError("Photo", "Photo type is incorrect");
                    return View();
                }
                if (!serviceVM.Photo.ValidateFileSize(SizeHelper.gb))
                {
                    ModelState.AddModelError("Photo", "Photo size is incorrect");
                    return View();
                }
                string filename = Guid.NewGuid().ToString() + serviceVM.Photo.FileName;
                string path = Path.Combine(_env.WebRootPath, "assets", "Landscaper", "img", "services", filename);
                FileStream fileStream = new FileStream(path, FileMode.Create);
                await serviceVM.Photo.CopyToAsync(fileStream);
                existed.Image.DeleteFile(path);
                existed.Image = filename;
            }
            existed.Title = serviceVM.Title;
            existed.Description = serviceVM.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Service existed = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (existed == null)
            {
                return NotFound();
            }
            _context.Services.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
