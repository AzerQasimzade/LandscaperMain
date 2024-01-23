using LandscaperProject.Areas.ViewModels.Service;
using LandscaperProject.Areas.ViewModels.Setting;
using LandscaperProject.DAL;
using LandscaperProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LandscaperProject.Areas.LandscaperAdmin.Controllers
{
    [Area("LandscaperAdmin")]

    public class SettingController : Controller
    {
        private readonly AppDbContext _context;

        public SettingController(AppDbContext context)
        {
            _context = context;
        }
        public  IActionResult Index()
        {
            List<Setting> settings = _context.Settings.ToList();
            return View(settings);
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Setting existed = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (existed == null)
            {
                return NotFound();
            }
            UpdateSettingVM serviceVM = new UpdateSettingVM
            {
                Value = existed.Value,
            };
            return View(serviceVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateSettingVM settingVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Setting existed = await _context.Settings.FirstOrDefaultAsync(c => c.Id == id);
            existed.Value=settingVM.Value;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Setting existed = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (existed == null)
            {
                return NotFound();
            }
            _context.Settings.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
