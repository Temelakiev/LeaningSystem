using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LearningSystem.Web.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string ToFriendlyUrl(this string text)
        {
            return Regex.Replace(text, @"[^A-Za-z0-9\.~]+", "-").ToLower();
        }
    }
}
