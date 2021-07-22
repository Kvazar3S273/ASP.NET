using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebAppSite.Domain;
using WebAppSite.Domain.Entities.Catalog;
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
            
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(AnimalCreateViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            DateTime dt = DateTime.Parse(model.BirthDay, new CultureInfo("uk-UA"));
            Animal animal = new Animal
            {
                Name = model.Name,
                DateBirth = dt,
                Image = model.Image,
                Price = model.Price,
                DateCreate = DateTime.Now
            };
            _context.Animals.Add(animal);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(long id)
        {
            var result = _context.Animals.FirstOrDefault(a => a.Id == id);
            return View(new AnimalCreateViewModel()
            {
                Name = result.Name,
                BirthDay = result.DateBirth.ToString(),
                Image = result.Image,
                Price = result.Price
            });
        }
        [HttpPost]
        public IActionResult Edit(long id, AnimalCreateViewModel model)
        {
            DateTime dt = DateTime.Parse(model.BirthDay, new CultureInfo("uk-UA"));
            if (ModelState.IsValid)
            {
                var result = _context.Animals.FirstOrDefault(a => a.Id == id);
                result.Name = model.Name;
                result.DateBirth = dt;
                result.Image = model.Image;
                result.Price = model.Price;
                _context.SaveChanges();
            };
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public IActionResult ConfirmDelete(long id)
        {
            var selectedItem = _context.Animals.FirstOrDefault(si => si.Id == id);
            if (selectedItem != null)
            {
                return View(selectedItem);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var deletedItem = _context.Animals.FirstOrDefault(di => di.Id == id);
            if (deletedItem != null)
            {
                _context.Animals.Remove(deletedItem);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

    }
}
