using AutoMapper;
using LearningSystem.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearningSystem.Test
{
    public class TestStartup
    {
        private static bool testsInitialize = false;

        public static void Initialize()
        {
            if (!testsInitialize)
            {
                Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());
                testsInitialize = true;
            }
        }
    }
}
