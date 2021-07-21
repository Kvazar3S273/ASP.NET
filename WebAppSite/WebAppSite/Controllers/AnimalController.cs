using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppSite.Domain;
using WebAppSite.Models;

namespace WebAppSite.Controllers
{
    public class AnimalController : Controller
    {
        private readonly AppEFContext _context;
        public AnimalController(AppEFContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<AnimalsViewModel> model =
                _context.Animals.Select(x => new AnimalsViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                    Image = x.Image,
                    BirthDay = x.DateBirth
                }).ToList();
                //new List<AnimalsViewModel>();
            ///model.Add(new AnimalsViewModel
            ///{
            ///    Id=1,
            ///    BirthDay=DateTime.Now,
            ///    Price=200,
            ///    Name="Ондатра Сара",
            ///    Image="1.jpg"
            ///});
            ///model.Add(new AnimalsViewModel
            ///{
            ///    Id = 2,
            ///    BirthDay = DateTime.Now,
            ///    Price = 200,
            ///    Name = "Павіван Бора",
            ///    Image = "2.jpg"
            ///});
            ///model.Add(new AnimalsViewModel
            ///{
            ///    Id = 3,
            ///    BirthDay = DateTime.Now,
            ///    Price = 200,
            ///    Name = "Павіван Жора",
            ///    Image = "3.jpg"
            ///});
            
            return View(model);
        }
    }
}
