using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LearningSystem.Services.Models;
using LearningSystem.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using LearningSystem.Data.Models;

namespace LearningSystem.Services.Implementations
{
    public class TrainerService : ITrainerService
    {
        private readonly LearningSystemDbContext db;

        public TrainerService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddGrade(int courseId, string studentId, Grade grade)
        {
            var studentInCourse = await this.db.FindAsync<StudentCourse>(courseId, studentId);

            if (studentInCourse==null)
            {
                return false;
            }

            studentInCourse.Grade = grade;

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CourseListingServiceModel>> ByTrainer(string trainerId)
            => await this.db
            .Courses
            .Where(c => c.Trainer.Id == trainerId)
            .ProjectTo<CourseListingServiceModel>()
            .ToListAsync();

        public async Task<bool> IsTrainer(int courseId, string trainerId)
            => await this.db
            .Courses
            .AnyAsync(c => c.Id == courseId && c.TrainerId == trainerId);


        public async Task<IEnumerable<StudentInCourseServiceModel>> StudentInCourseAsync(int courseId)
            => await this.db
            .Courses
            .Where(c => c.Id == courseId)
            .SelectMany(c => c.Students.Select(s => s.Student))
            .ProjectTo<StudentInCourseServiceModel>(new { courseId })
            .ToListAsync();
    }
}
