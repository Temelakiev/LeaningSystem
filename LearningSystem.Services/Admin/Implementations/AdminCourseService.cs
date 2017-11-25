using LearningSystem.Data;
using LearningSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSystem.Services.Admin.Implementations
{
    public class AdminCourseService : IAdminCourseService
    {
        private readonly LearningSystemDbContext db;

        public AdminCourseService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public  async Task Create(string name, string description, DateTime strarDate, DateTime endDate, string trainerId)
        {
            var course = new Course
            {
                Name = name,
                Description = description,
                StartDate = strarDate,
                EndDate = endDate,
                TrainerId = trainerId
            };

            this.db.Add(course);
            await this.db.SaveChangesAsync();
        }
    }
}
