using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LearningSystem.Data.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime Birthdate { get; set; }

        public List<Course> Trainings { get; set; } = new List<Course>();

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<Article> Articles { get; set; } = new List<Article>();

        public List<StudentCourse> Courses { get; set; } = new List<StudentCourse>();
    }
}
