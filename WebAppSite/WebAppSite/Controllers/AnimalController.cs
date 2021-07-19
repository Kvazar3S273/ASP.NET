using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppSite.Models;

namespace WebAppSite.Controllers
{
    public class AnimalController : Controller
    {
        public IActionResult Index()
        {
            List<AnimalsViewModel> model = new List<AnimalsViewModel>();
            model.Add(new AnimalsViewModel
            {
                Id=1,
                BirthDay=DateTime.Now,
                Price=200,
                Name="Ондатра Сара",
                Image="1.jpg"
            });
            model.Add(new AnimalsViewModel
            {
                Id = 2,
                BirthDay = DateTime.Now,
                Price = 200,
                Name = "Павіван Бора",
                Image = "2.jpg"
            });
            model.Add(new AnimalsViewModel
            {
                Id = 3,
                BirthDay = DateTime.Now,
                Price = 200,
                Name = "Павіван Жора",
                Image = "3.jpg"
            });
            return View(model);
        }
    }
}
