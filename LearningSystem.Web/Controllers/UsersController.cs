using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LearningSystem.Services;
using Microsoft.AspNetCore.Identity;
using LearningSystem.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace LearningSystem.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService users;
        private readonly UserManager<User> userManager;

        public UsersController(IUserService users,UserManager<User> userManager)
        {
            this.users = users;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Profile(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);

            if (user==null)
            {
                return NotFound();
            }

            var profile = await this.users.ProfileAsync(user.Id);

            return View(profile);
        }

        [Authorize]
        public async Task<IActionResult> DownloadCertificate(int id)
        {
            var userId = this.userManager.GetUserId(User);
            var certificateContents = await this.users.GetPdfCertificate(id, userId);

            if (certificateContents==null)
            {
                return BadRequest();
            }

            return File(certificateContents, "application/pdf", "Certificate.pdf");
        }
    }
}
