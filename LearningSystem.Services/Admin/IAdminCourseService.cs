using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSystem.Services.Admin
{
    public interface IAdminCourseService
    {
        Task Create(string name, string description, DateTime strarDate, DateTime endDate, string trainerId);
    }
}
