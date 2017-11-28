using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LearningSystem.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using LearningSystem.Web.Areas.Admin.Models.Courses;
using LearningSystem.Web.Controllers;
using LearningSystem.Services.Admin;
using LearningSystem.Web.Infrastructure.Extensions;

namespace LearningSystem.Web.Areas.Admin.Controllers
{
    public class CoursesController : BaseAdminController
    {
        private readonly UserManager<User> userManager;
        private readonly IAdminCourseService courses;

        public CoursesController(UserManager<User> userManager, IAdminCourseService courses)
        {
            this.userManager = userManager;
            this.courses = courses;
        }

        public async Task<IActionResult> Create()
        {
            return View(new AddCourseFormModel
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                Trainers = await this.GetTrainers()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddCourseFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Trainers = await this.GetTrainers();
                return View(model);
            }

            var endDate = model.EndDate;

            await this.courses.Create(
                model.Name, 
                model.Description, 
                model.StartDate, 
                new DateTime(endDate.Year, endDate.Month, endDate.Day, 23,59,59), model.TrainerId);

            TempData.AddSuccuessMessage($"Course {model.Name} created successfully");

            return RedirectToAction(nameof(HomeController.Index), "Home", new { area = string.Empty });
        }

        private async Task<IEnumerable<SelectListItem>> GetTrainers()
        {
            var trainers = await this.userManager
             .GetUsersInRoleAsync(WebConstants.TrainerRole);

            var trainerListItems = trainers.Select(t => new SelectListItem
            {
                Text = t.UserName,
                Value = t.Id
            })
            .ToList();

            return trainerListItems;
        }
    }
}
