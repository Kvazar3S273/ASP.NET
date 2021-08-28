using AutoMapper;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
        private readonly IMapper _mapper;
        private IHostEnvironment _host;
        public AnimalController(AppEFContext context, IMapper mapper, IHostEnvironment host)
        {
            _context = context;
            _mapper = mapper;
            _host = host;
            //GenerateAnimal();
        }

        private void GenerateAnimal()
        {
            int n = 100;
            var endDate = DateTime.Now;
            var startDate = new DateTime(endDate.Year - 2, endDate.Month, endDate.Day);

            var faker = new Faker<Animal>("uk")
                .RuleFor(x => x.Name, f => f.Person.FirstName)
                .RuleFor(x => x.DateBirth, f => f.Date.Between(startDate, endDate))
                .RuleFor(x => x.Image, f => f.Image.PicsumUrl())
                .RuleFor(x => x.Price, f => Decimal.Parse(f.Commerce.Price(100M, 500M)))
                .RuleFor(x => x.DateCreate, DateTime.Now);

            for (int i = 0; i < n; i++)
            {
                var animal = faker.Generate();
                _context.Animals.Add(animal);
                _context.SaveChanges();
            }
        }

        public IActionResult Index(SearchHomeIndexModel search, int page = 1)
        {
            int showItems = 7;
            var query = _context.Animals.AsQueryable();
            if(!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(x => x.Name.Contains(search.Name));
            }
            HomeIndexModel model = new HomeIndexModel();

            // кількість записів, які знайшли в загальному
            int countItems = query.Count();
            var pageCount = (int)Math.Ceiling(countItems/(double)showItems);
            if (pageCount == 0) pageCount = 1;

            int skipItems = (page - 1) * showItems;
            query = query.Skip(skipItems).Take(showItems);

            model.Animals = query
                .Select(x => _mapper.Map<AnimalsViewModel>(x))
                .ToList();
            model.Search = search;
            model.Page = page;
            model.PageCount = pageCount;
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AnimalCreateViewModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            string fileName = "";
            if (model.Image!=null)
            {
                var ext = Path.GetExtension(model.Image.FileName);
                fileName = Path.GetRandomFileName() + ext;
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                var filePath = Path.Combine(dir, fileName);
                
                using(var stream=System.IO.File.Create(filePath))
                {
                    await model.Image.CopyToAsync(stream);
                }
            }
            DateTime dt = DateTime.Parse(model.BirthDay, new CultureInfo("uk-UA"));
            Animal animal = new Animal
            {
                Name = model.Name,
                DateBirth = dt,
                Image = fileName,
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
            AnimalCreateViewModel animal = new AnimalCreateViewModel();
            var result = _context.Animals.FirstOrDefault(a => a.Id == id);
            if (result.Image != null)
            {
                var name = Path.GetFileName(result.Image);
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "images");
                var filePath = Path.Combine(dir, name);

                using (var stream = System.IO.File.OpenRead($"{filePath}"))
                {
                    var resultImage = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
                    animal.Name = result.Name;
                    animal.Price = result.Price;
                    animal.BirthDay = result.DateBirth.ToString();
                    animal.Image = resultImage;
                }
            }
            return View(animal);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(long id, AnimalCreateViewModel model)
        {
            DateTime dt = DateTime.Parse(model.BirthDay, new CultureInfo("uk-UA"));
            if (ModelState.IsValid)
            {
                var result = _context.Animals.FirstOrDefault(a => a.Id == id);
                result.Name = model.Name;
                result.DateBirth = dt;
                //result.Image = model.Image;
                result.Price = model.Price;
                string fileName = string.Empty;
                if(model.Image!=null)
                {
                    var ext = Path.GetExtension(model.Image.FileName);
                    fileName = Path.GetRandomFileName() + ext;
                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "images");
                    var pathFile = Path.Combine(directory, fileName);
                    using(var stream = System.IO.File.Create(pathFile))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                    var beforeImg = result.Image;
                    string folder = "\\images\\";
                    string path = _host.ContentRootPath + folder + beforeImg;
                    if(System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    result.Image = fileName;
                }
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
                var fileName = deletedItem.Image;
                string dir = "\\images\\";
                string path = _host.ContentRootPath + dir + fileName;
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                _context.Animals.Remove(deletedItem);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
            //return Ok();
        }

    }
}
