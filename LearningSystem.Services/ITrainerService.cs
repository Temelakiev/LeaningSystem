using LearningSystem.Data.Models;
using LearningSystem.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSystem.Services
{
    public interface ITrainerService
    {
        Task<IEnumerable<CourseListingServiceModel>> ByTrainer(string trainerId);

        Task<bool> IsTrainer(int courseId, string trainerId);

        Task<IEnumerable<StudentInCourseServiceModel>> StudentInCourseAsync(int courseId);

        Task<bool> AddGrade(int courseId, string studentId, Grade grade);
    }
}
