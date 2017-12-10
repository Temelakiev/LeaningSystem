using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using System.Threading.Tasks;
using LearningSystem.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using LearningSystem.Data;
using LearningSystem.Data.Models;
using System.Linq;
using AutoMapper;
using LearningSystem.Web.Infrastructure.Mapping;

namespace LearningSystem.Test.Services
{
    public class CourseServiceTest
    {
        public CourseServiceTest()
        {
            TestStartup.Initialize();
        }

        [Fact]
        public async Task FindAsyncShouldReturnCorrectResultsWithFilterAndOrdering()
        {
            // Arrange
            var db = this.GetDatabase();

            var firstCourse = new Course { Id = 1, Name = "First" };
            var secondCourse = new Course { Id = 2, Name = "Second" };
            var thirdCourse = new Course { Id = 3, Name = "Third" };

            db.AddRange(firstCourse, secondCourse, thirdCourse);

            await db.SaveChangesAsync();

            var courseService = new CourseService(db);
            // Act
            var result = await courseService.FindAsync("t");


            // Assert
            result
                .Should()
                .Match(r => r.ElementAt(0).Id == 3
                 && r.ElementAt(1).Id == 1)
                .And
                .HaveCount(2);

        }

        [Fact]
        public async Task SignUpStudentAsyncShouldSaveCorrectDataWithValidCourseIdAndStudentId()
        {
            // Arrange
            var db = this.GetDatabase();

            const int courseId = 1;
            const string studentId = "TestStudentId";

            var course = new Course
            {
                Id = courseId,
                StartDate = DateTime.MaxValue,
                Students = new List<StudentCourse>()
            };

            db.Add(course);

            await db.SaveChangesAsync();

            var courseService = new CourseService(db);
            // Act
            var result = await courseService.SignUpUser(courseId, studentId);
            var savedEntry = db.Find<StudentCourse>(courseId, studentId);

            // Assert
            result
                .Should()
                .Be(true);

            savedEntry
                .Should()
                .NotBeNull();


        }

        private LearningSystemDbContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<LearningSystemDbContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            return new LearningSystemDbContext(dbOptions);
        }
    }
}
