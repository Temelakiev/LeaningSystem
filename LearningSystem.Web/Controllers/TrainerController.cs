using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LearningSystem.Services;
using Microsoft.AspNetCore.Identity;
using LearningSystem.Data.Models;
using LearningSystem.Web.Models.Trainer;
using LearningSystem.Services.Models;

namespace LearningSystem.Web.Controllers
{
    [Authorize(Roles = WebConstants.TrainerRole)]
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainers;
        private readonly UserManager<User> userManager;
        private readonly ICourseService courses;

        public TrainerController(ITrainerService trainers, UserManager<User> userManager, ICourseService courses)
        {
            this.trainers = trainers;
            this.userManager = userManager;
            this.courses = courses;
        }

        public async Task<IActionResult> Courses()
        {
            var userId = this.userManager.GetUserId(User);
            var courses = await this.trainers.ByTrainer(userId);

            return View(courses);
        }

        public async Task<IActionResult> Students(int id)
        {
            var userId = this.userManager.GetUserId(User);

            if (!await this.trainers.IsTrainer(id, userId))
            {
                return NotFound();
            }


            return View(new StudentsInCourseViewModel
            {
                Students = await this.trainers.StudentInCourseAsync(id),
                Course = await this.courses.ByIdAsync<CourseListingServiceModel>(id)
            });
        }

        [HttpPost]
        public async Task<IActionResult> GradeStudent(int id, string studentId, Grade grade)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest();
            }

            var userId = this.userManager.GetUserId(User);

            if (!await this.trainers.IsTrainer(id, userId))
            {
                return BadRequest();
            }

            var success = await this.trainers.AddGrade(id, studentId, grade);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(Students), new { id });
        }

        public async Task<IActionResult> DownloadExam(int id,string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
            {
                return BadRequest();
            }

            var userId = this.userManager.GetUserId(User);

            if (!await this.trainers.IsTrainer(id, userId))
            {
                return BadRequest();
            }

            var examContents = await this.trainers.GetExamSubmission(id, studentId);

            if (examContents == null)
            {
                return BadRequest();
            }

            var studentInCourseNames = await this.trainers.StudentInCourseNamesAsync(id, studentId);

            if (studentInCourseNames == null)
            {
                return BadRequest();
            }

            return File(examContents, "application/zip",$"{studentInCourseNames.CourseTitle} - {studentInCourseNames.Username}-{DateTime.UtcNow.ToShortDateString()}.zip");
        }
    }
}
