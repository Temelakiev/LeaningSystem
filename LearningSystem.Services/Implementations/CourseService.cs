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
    public class CourseService : ICourseService
    {
        private readonly LearningSystemDbContext db;

        public CourseService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<CourseListingServiceModel>> ActiveAsync()
            => await this.db
            .Courses
            .OrderByDescending(c => c.Id)
            .Where(c => c.StartDate >= DateTime.UtcNow)
            .ProjectTo<CourseListingServiceModel>()
            .ToListAsync();

        public async Task<CourseDetailsServiceModel> ByIdAsync(int id)
            => await this.db
            .Courses
            .Where(c => c.Id == id)
            .ProjectTo<CourseDetailsServiceModel>()
            .FirstOrDefaultAsync();

        public async Task<bool> SignOutUser(int courseId, string userId)
        {
            var courseInfo = await this.GetCourseInfo(courseId, userId);

            if (courseInfo == null || courseInfo.StartDate < DateTime.UtcNow || !courseInfo.UserIsSignedIn)
            {
                return false;
            }

            var studentInCourse = await this.db
                .FindAsync(typeof(StudentCourse),
               courseId,userId);

            this.db.Remove(studentInCourse);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SignUpUser(int courseId, string userId)
        {
            var courseInfo = await this.GetCourseInfo(courseId, userId);

            if (courseInfo == null || courseInfo.StartDate < DateTime.UtcNow || courseInfo.UserIsSignedIn)
            {
                return false;
            }

            var studentInCourse = new StudentCourse
            {
                CourseId = courseId,
                StudentId = userId
            };

            this.db.Add(studentInCourse);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UserIsSignedInCourse(int courseId, string userId)
            => await this.db
            .Courses
            .AnyAsync(c => c.Id == courseId && c.Students.Any(s => s.StudentId == userId));

        private async Task<CourseInfo> GetCourseInfo(int courseId, string userId)
            => await this.db
                .Courses
                .Where(c => c.Id == courseId)
                .Select(c => new CourseInfo
                {
                    StartDate = c.StartDate,
                    UserIsSignedIn = c.Students.Any(s => s.StudentId == userId)
                })
            .FirstOrDefaultAsync();
    }
}
