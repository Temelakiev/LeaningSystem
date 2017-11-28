using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningSystem.Web.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using LearningSystem.Services;
using Microsoft.AspNetCore.Identity;
using LearningSystem.Data.Models;
using LearningSystem.Web.Models.Courses;
using Microsoft.AspNetCore.Authorization;
using LearningSystem.Services.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LearningSystem.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService courses;
        private readonly UserManager<User> userManager;

        public CoursesController(ICourseService courses, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.courses = courses;
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = new CourseDetailsViewModel
            {
                Course = await this.courses.ByIdAsync<CourseDetailsServiceModel>(id),

            };

            if (model.Course==null)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var userId = this.userManager.GetUserId(User);

                model.UserIsSignedInCourse = await this.courses.UserIsSignedInCourse(id, userId);
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignUp(int id)
        {
            var userId = this.userManager.GetUserId(User);

            var result = await this.courses.SignUpUser(id, userId);

            if (!result)
            {
                return BadRequest();
            }

            TempData.AddSuccuessMessage("Thank you for your registrtion!");

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignOut(int id)
        {
            var userId = this.userManager.GetUserId(User);

            var result = await this.courses.SignOutUser(id, userId);

            if (!result)
            {
                return BadRequest();
            }

            TempData.AddSuccuessMessage("Sorry to see you go!");

            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitExam(int id,IFormFile exam)
        {
            if (exam.FileName.EndsWith(".zip") || exam.Length > 2 * 1024 * 1024)
            {
                TempData.AddErrorMessage("Your submission should be a '.zip' file with no more than 2 MB in size!");
                return RedirectToAction(nameof(Details), new { id });
            }

            var fileContents = await exam.ToByteArrayAsync();
            var userId = this.userManager.GetUserId(User);

            var success =await this.courses.SaveExamSubmission(id, userId, fileContents);

            if (!success)
            {
                return BadRequest();
            }

            TempData.AddSuccuessMessage("Exam submission saved successfully!");
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
