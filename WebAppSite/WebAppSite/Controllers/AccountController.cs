using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAppSite.Domain.Entities.Identity;
using WebAppSite.Models;

namespace WebAppSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private IHostEnvironment _host;


        public AccountController(UserManager<AppUser> userManager, 
                                SignInManager<AppUser> signInManager,
                                RoleManager<AppRole> roleManager,
                                IHostEnvironment host)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _host = host;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user!=null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, 
                        model.Password, false, false);
                    if(result.Succeeded)
                        return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Дані вказано не коректно");
            return View(model);
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            string fileNameUser = string.Empty;

            //якщо фото обрано:
            if (model.Image != null)
            {
                var ext = Path.GetExtension(model.Image.FileName);
                fileNameUser = Path.GetRandomFileName() + ext;
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                var filePath = Path.Combine(dir, fileNameUser);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await model.Image.CopyToAsync(stream);
                }
            }


            if (user != null)
            {
                ModelState.AddModelError("Email", "Дана пошта вже зареєстрована. Спробуйте іншу");
            }

            if (ModelState.IsValid)
            {
                user = new AppUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    ImageProfile=fileNameUser
                };
                var role = new AppRole
                {
                    Name = "User"
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Щось пішло не за планом");
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
