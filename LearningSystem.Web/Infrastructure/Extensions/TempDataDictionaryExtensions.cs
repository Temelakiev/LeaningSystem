using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningSystem.Web.Infrastructure.Extensions
{
    public static class TempDataDictionaryExtensions
    {
        public static void AddSuccuessMessage(this ITempDataDictionary tempData,string message)
        {
            tempData[WebConstants.TempDataSuccessMessageKey] = message;
        }
    }
}
