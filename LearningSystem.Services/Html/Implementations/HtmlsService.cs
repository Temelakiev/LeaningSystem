using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSystem.Services.Html.Implementations
{
    public class HtmlsService : IHtmlsService
    {
        private readonly HtmlSanitizer htmlSanitizer;

        public HtmlsService()
        {
            this.htmlSanitizer = new HtmlSanitizer();
            this.htmlSanitizer.AllowedAttributes.Add("class");
        }

        public string Sanitize(string htmlContent)
            => this.htmlSanitizer.Sanitize(htmlContent);
    }
}
