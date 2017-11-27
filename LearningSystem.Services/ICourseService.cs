using LearningSystem.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSystem.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseListingServiceModel>> ActiveAsync();

        Task<CourseDetailsServiceModel> ByIdAsync(int id);

        Task<bool> UserIsSignedInCourse(int courseId, string userId);

        Task<bool> SignUpUser(int courseId, string userId);

        Task<bool> SignOutUser(int courseId, string userId);
    }
}
