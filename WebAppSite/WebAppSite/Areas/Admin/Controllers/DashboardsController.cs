using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAppSite.Areas.Admin.Models;
using WebAppSite.Domain;
using WebAppSite.Domain.Entities.Identity;

namespace WebAppSite.Areas.Admin.Controllers
{
    [Area("admin")]
    public class DashboardsController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppEFContext _context;
        public AppRole role { get; set; }
        public DashboardsController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, AppEFContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> Dashboard_0()
        {
            var userRole = (from user in _context.Users
                                  select new{
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      RoleNames = (from userRole in user.UserRoles
                                                   join role in _context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()})
                                                   .ToList()
                                                   .Select(p => new RolesViewModel(){
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Role = string.Join(",", p.RoleNames)
                                  });

            return await Task.FromResult(View(userRole));
        }

        //Додавання нового користувача
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserModel newUser)
        {
            var user = await _userManager.FindByEmailAsync(newUser.Email);
            string fileName = string.Empty;
            if (newUser.Image != null)
            {
                var ext = Path.GetExtension(newUser.Image.FileName);
                fileName = Path.GetRandomFileName() + ext;
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                var filePath = Path.Combine(dir, fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await newUser.Image.CopyToAsync(stream);
                }
            }

            if (user != null)
            {
                ModelState.AddModelError("Email", "Такий користувач вже існує");
            }

            if (ModelState.IsValid)
            {
                user = new AppUser
                {
                    Email = newUser.Email,
                    UserName = newUser.Email,
                    ImageProfile = fileName
                };
                var role = new AppRole
                {
                    Name = "User"
                };

                var result = await _userManager.CreateAsync(user, newUser.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Dashboard_0", "Dashboards");
                }
                else
                {
                    ModelState.AddModelError("", "Щось пішло не за планом");
                }
            }
            
            return View(newUser);
        }

        //Редагування користувача

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            EditUserModel editUser = new EditUserModel();

            var userRole = (from us in _context.Users
                                  select new
                                  {
                                      UserId = us.Id,
                                      Username = us.UserName,
                                      Email = us.Email,
                                      RoleNames = (from userRole in us.UserRoles
                                                   join role in _context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new RolesViewModel()

                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      Role = string.Join(",", p.RoleNames)
                                  });
            foreach (var item in userRole)
            {
                editUser.RoleUser = item.Role;
            }
            if (user != null)
            {
                editUser.NameUser = user.NormalizedUserName;
                editUser.Email = user.Email;
                editUser.Image = user.ImageProfile;
                return View(editUser);
            }
            else
                return RedirectToAction("Dashboard_0", "Dashboards");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string image)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("", "Поле Email не може бути порожнім");


                if (user.ImageProfile != null)
                    user.ImageProfile = image;
                else
                    ModelState.AddModelError("", "Виберіть фото");

                if (!string.IsNullOrEmpty(email) && user.ImageProfile != null)
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Dashboard_0", "Dashboards");
                    else
                        ModelState.AddModelError("", "Щось не то...");
                }
            }
            else
                ModelState.AddModelError("", "Не знайдено такого користувача");
            return View(user);
        }



        //Видалення користувача
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Dashboard_0");
                else
                    ModelState.AddModelError("", "Не можливо видалити користувача");
            }
            else
                ModelState.AddModelError("", "Не вдалося знайти користувача");

            return View("Dashboard_0", _userManager.Users);
        }

        public IActionResult Dashboard_1()
        {
            return View();
        }

        public IActionResult Dashboard_2()
        {
            return View();
        }

        public IActionResult Dashboard_3()
        {
            return View();
        }

        public IActionResult Dashboard_4()
        {
            return View();
        }

        public IActionResult Dashboard_4_1()
        {
            return View();
        }

        public IActionResult Dashboard_5()
        {
            return View();
        }
    }
}
