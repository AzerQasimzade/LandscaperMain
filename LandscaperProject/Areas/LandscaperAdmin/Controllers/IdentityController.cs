using LandscaperProject.Areas.ViewModels.Account;
using LandscaperProject.Models;
using LandscaperProject.Utilities.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LandscaperProject.Areas.LandscaperAdmin.Controllers
{
    [Area("LandscaperAdmin")]
    public class IdentityController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser
            {
                UserName = registerVM.Username,
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname = registerVM.Surname
            };
            IdentityResult identityResult = await _userManager.CreateAsync(user, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                    return View();
                }
            }
            await _userManager.AddToRoleAsync(user,UserRoles.Admin.ToString());
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser existed = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
            if (existed is null)
            {
                existed = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
                if (existed is null)
                {
                    ModelState.AddModelError(String.Empty, "Username,Email or Password is wrong");
                    return View();
                }
            }
            var passResult=await _signInManager.PasswordSignInAsync(existed, loginVM.Password, loginVM.IsRemembered,false);
            if (!passResult.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Username,Email or Password is wrong");
                return View();
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        //public async Task<IActionResult> CreateRole()
        //{
        //    foreach (var item in Enum.GetValues(typeof(UserRoles)))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole
        //        {
        //            Name = item.ToString(),
        //        });
        //    }
        //    return RedirectToAction("Index", "Home", new { area = "" });
        //}
    }
}
