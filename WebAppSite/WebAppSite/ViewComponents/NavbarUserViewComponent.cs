using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppSite.Domain.Entities.Identity;
using WebAppSite.Models;

namespace WebAppSite.ViewComponents
{
    [Authorize]
    public class NavbarUserViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        public NavbarUserViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            UserNavbarInfoViewModel model = new UserNavbarInfoViewModel
            {
                FullName = "Павіан Жора",
                Image = "https://static.educalingo.com/img/ms/800/primat.jpg"
            };
            return View("_UserNavbarInfo", model);
        }
    }
}
