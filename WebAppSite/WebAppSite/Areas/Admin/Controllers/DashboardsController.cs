using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        //[HttpPost]
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
