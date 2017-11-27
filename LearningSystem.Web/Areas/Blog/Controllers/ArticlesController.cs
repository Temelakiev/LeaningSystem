using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LearningSystem.Web.Areas.Blog.Models.Articles;
using Ganss.XSS;
using LearningSystem.Services.Html;
using LearningSystem.Services.Blog;
using Microsoft.AspNetCore.Identity;
using LearningSystem.Data.Models;

namespace LearningSystem.Web.Areas.Blog.Controllers
{
    [Area(WebConstants.BlogArea)]
    [Authorize(Roles = WebConstants.BlogAuthorRole)]
    public class ArticlesController : Controller
    {
        private readonly IHtmlsService html;
        private readonly IBlogArticleService articles;
        private readonly UserManager<User> userManager;

        public ArticlesController(IHtmlsService html, IBlogArticleService articles, UserManager<User> userManager)
        {
            this.html = html;
            this.articles = articles;
            this.userManager = userManager;
        }
        
        public IActionResult Create()
            => View();

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page=1)
            => View(new ArticleViewListingModel
            {
                Articles=await this.articles.AllAsync(page),
                TotalArticles=await this.articles.TotalAsync(),
                CurrentPage=page
            });

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
            => View(await this.articles.ById(id));

      

        [HttpPost]
        public async Task<IActionResult> Create(PublishArticleFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Content = this.html.Sanitize(model.Content);

            var userId = this.userManager.GetUserId(User);

            await this.articles.CreateAsync(model.Title, model.Content, userId);

            return RedirectToAction(nameof(Index));
        }
    }
}
