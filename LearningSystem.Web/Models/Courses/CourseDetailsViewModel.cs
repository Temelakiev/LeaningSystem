using LearningSystem.Common.Mapping;
using LearningSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LearningSystem.Services.Models;

namespace LearningSystem.Web.Models.Courses
{
    public class CourseDetailsViewModel
    {
        public CourseDetailsServiceModel Course { get; set; }

        public bool UserIsSignedInCourse { get; set; }
    }
}
