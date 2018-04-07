using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HandlebarsViewEngine.HandlebarsConstants;

namespace HandlebarsViewEngine
{
    public class HandlebarsViewEngineOptionsSetup : ConfigureOptions<HandlebarsViewEngineOptions>
    {
        public HandlebarsViewEngineOptionsSetup() : base(Configure) { }

        private new static void Configure(HandlebarsViewEngineOptions options)
        {
            options.ViewLocationFormats.Add("Views/{1}/{0}" + VIEW_EXTENSION);
            options.ViewLocationFormats.Add("Views/Shared/{0}" + VIEW_EXTENSION);
            options.DefaultLayout = "Views/Shared/layout.hbs";
        }
    }
}
