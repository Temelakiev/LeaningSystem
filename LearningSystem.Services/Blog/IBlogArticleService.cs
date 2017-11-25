using LearningSystem.Services.Blog.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LearningSystem.Services.Blog
{
    public interface IBlogArticleService
    {
        Task CreateAsync(string title, string content, string authorId);

        Task<int> TotalAsync();

        Task<IEnumerable<BlogArticleListingServiceModel>> AllAsync(int page=1);

        Task<BlogArticleDetailsServiceModel> ById(int id);
    }
}
