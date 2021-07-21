using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppSite.Models
{
    public class AnimalsViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
    #region Animal Create
    public class AnimalCreateViewModel
    {
        [Display(Name="Назва тварини")]
        public string Name { get; set; }

        [Display(Name="Дата народження")]
        public string BirthDay { get; set; }

        [Display(Name="Ціна")]
        public decimal Price { get; set; }

        [Display(Name="Зображення")]
        public string Image { get; set; }
    }

    public class AnimalCreateValidator : AbstractValidator<AnimalCreateViewModel>
    {
        public AnimalCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Поле не може бути порожнім");
            RuleFor(x => x.BirthDay).NotEmpty()
                .WithMessage("Поле не може бути порожнім");
            RuleFor(x => x.Price).NotEmpty()
                .WithMessage("Поле не може бути порожнім");
        }
    }
    #endregion
}