using System.Threading.Tasks;
using LearningSystem.Services.Models;
using LearningSystem.Data;
using System.Linq;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using LearningSystem.Data.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using System.IO;
using iTextSharp.text.pdf;
using System;

namespace LearningSystem.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly LearningSystemDbContext db;
        private readonly IPdfGenerator pdfGenerator;

        public UserService(LearningSystemDbContext db,IPdfGenerator pdfGenerator)
        {
            this.db = db;
            this.pdfGenerator = pdfGenerator;
        }

        public async Task<IEnumerable<UserListingServiceModel>> FindAsync(string searchText)
        {
            searchText = searchText ?? string.Empty;

            return await this.db
            .Users
            .OrderBy(u => u.UserName)
            .Where(u => u.Name.ToLower().Contains(searchText.ToLower()))
            .ProjectTo<UserListingServiceModel>()
            .ToListAsync();
        }

        public async Task<byte[]> GetPdfCertificate(int id,string studentId)
        {
            var studentInCourse= await this.db
                .FindAsync<StudentCourse>(id,studentId);

            if (studentInCourse==null)
            {
                return null;
            }

            var data = await this.db
                .Courses
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    CourseName=c.Name,
                    CourseStartDate=c.StartDate,
                    CourseEndDate=c.EndDate,
                    StudentName=c.Students.Where(s=>s.StudentId==studentId).Select(s=>s.Student.Name).FirstOrDefault(),
                    StudentGrade=c.Students.Where(s => s.StudentId == studentId).Select(s => s.Grade).FirstOrDefault(),
                    TrainerName=c.Trainer.Name
                })
                .FirstOrDefaultAsync();

            return this.pdfGenerator.GeneratePdfFromHtml(string.Format(ServiceConstants.PdfCertificateFormat,
                data.CourseName, data.CourseStartDate.ToShortDateString(), data.CourseEndDate.ToShortDateString(), 
                data.StudentName, data.StudentGrade, data.TrainerName,
                DateTime.UtcNow.ToShortDateString()));
        }

        public async Task<UserProfileServiceModel> ProfileAsync(string id)
            => await this.db
            .Users
            .Where(u => u.Id == id)
            .ProjectTo<UserProfileServiceModel>(new { studentId=id})
            .FirstOrDefaultAsync();
    }
}
