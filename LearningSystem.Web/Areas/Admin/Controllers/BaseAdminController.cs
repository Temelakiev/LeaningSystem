using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LearningSystem.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = WebConstants.AdministratorRole)]
    public abstract class BaseAdminController : Controller
    {
        
    }
}
