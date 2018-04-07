using System;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using HandlebarsViewEngine.Helpers;
using static HandlebarsViewEngine.HandlebarsConstants;

namespace HandlebarsViewEngine
{
    public class HandlebarsViewEngine : IHandlebarsViewEngine
    {
        private readonly HandlebarsViewEngineOptions _options;

        public HandlebarsViewEngine(IOptions<HandlebarsViewEngineOptions> optionsAccessor)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        }

        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
        {
            var controllerName = context.GetNormalizedRouteValue(CONTROLLER_KEY);
            var areaName = context.GetNormalizedRouteValue(AREA_KEY);
            var layoutPath = _options.DefaultLayout;

            var checkedLocations = new List<string>();
            foreach (var location in _options.ViewLocationFormats)
            {
                var viewPath = string.Format(location, viewName, controllerName);
                if (File.Exists(viewPath))
                {
                    return ViewEngineResult.Found("Default", new HandlebarsView(layoutPath, viewPath));
                }
                checkedLocations.Add(viewPath);
            }

            return ViewEngineResult.NotFound(viewName, checkedLocations);
        }

        public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
        {
            var applicationRelativePath = PathHelper.GetAbsolutePath(executingFilePath, viewPath);

            if (!PathHelper.IsAbsolutePath(viewPath))
            {
                // Not a path this method can handle.
                return ViewEngineResult.NotFound(applicationRelativePath, Enumerable.Empty<string>());
            }

            var layoutPath = _options.DefaultLayout;
            return ViewEngineResult.Found("Default", new HandlebarsView(layoutPath, applicationRelativePath));
        }
    }
}
