using System;
using System.Collections.Generic;
using System.Text;
using LearningSystem.Services.Admin.Models;
using LearningSystem.Data;
using AutoMapper.QueryableExtensions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LearningSystem.Services.Admin.Implementations
{
    public class AdminUserService : IAdminUserService
    {
        private readonly LearningSystemDbContext db;

        public AdminUserService(LearningSystemDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<AdminUserListingServiceModel>> AllAsync()
            =>await this.db.Users.ProjectTo<AdminUserListingServiceModel>().ToListAsync();
    }
}
