using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSystem.Services.Html
{
    public interface IHtmlsService
    {
        string Sanitize(string htmlContent);
    }
}
