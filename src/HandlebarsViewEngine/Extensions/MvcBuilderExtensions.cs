using HandlebarsViewEngine.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandlebarsViewEngine
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddHandlebars(this IMvcBuilder builder, Action<HandlebarsViewEngineOptions> setupAction = null)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddOptions()
                            .AddTransient<IConfigureOptions<HandlebarsViewEngineOptions>, HandlebarsViewEngineOptionsSetup>();

            if (setupAction != null)
            {
                builder.Services.Configure(setupAction);
            }

            builder.Services
                .AddTransient<IConfigureOptions<MvcViewOptions>, HandlebarsMvcViewOptionsSetup>()
                .AddSingleton<IHandlebarsViewEngine, HandlebarsViewEngine>();

            return builder;
        }
    }
}
