using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandlebarsViewEngine
{
    public class HandlebarsMvcViewOptionsSetup : IConfigureOptions<MvcViewOptions>
    {
        private readonly IHandlebarsViewEngine _HandlebarsViewEngine;

        /// <summary>
        /// Initializes a new instance of <see cref="HandlebarsMvcViewOptionsSetup"/>.
        /// </summary>
        /// <param name="HandlebarsViewEngine">The <see cref="IHandlebarsViewEngine"/>.</param>
        public HandlebarsMvcViewOptionsSetup(IHandlebarsViewEngine HandlebarsViewEngine)
        {
            _HandlebarsViewEngine = HandlebarsViewEngine ?? throw new ArgumentNullException(nameof(HandlebarsViewEngine));
        }

        /// <summary>
        /// Configures <paramref name="options"/> to use <see cref="HandlebarsViewEngine"/>.
        /// </summary>
        /// <param name="options">The <see cref="MvcViewOptions"/> to configure.</param>
        public void Configure(MvcViewOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            options.ViewEngines.Add(_HandlebarsViewEngine);
        }

    }
}
