using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HandlebarsViewEngine
{
    public static class HandlebarsCache
    {
        private static IDictionary<ViewDefinition, Func<object, string>> _views = new Dictionary<ViewDefinition, Func<object, string>>();
        private static readonly object _locker = new object();

        static HandlebarsCache()
        {
            // Register all partials
            List<string> partials = new List<string>();

            // Match all directories under "Views" with "Partials" in the name
            foreach (var directory in Directory.EnumerateDirectories("Views", "Partials", SearchOption.AllDirectories))
            {
                // Register all hbs files within the directory
                foreach (var path in Directory.EnumerateFiles(directory, "*.hbs", SearchOption.TopDirectoryOnly))
                {
                    string name = Path.GetFileNameWithoutExtension(path);
                    if (partials.Exists(p => p.Equals(name, StringComparison.OrdinalIgnoreCase)))
                        throw new Exception("Duplicate partial view: '" + name + "' (" + path + "). All partial views must be uniquely named");
                    else
                        partials.Add(path);
                    string partialTemplate = File.ReadAllText(path);
                    Handlebars.RegisterTemplate(name, partialTemplate);
                }
            }
        }

        public static Func<object, string> GetTemplate(string layoutPath, string viewPath)
        {
            lock(_locker)
            {
                var viewDef = new ViewDefinition(layoutPath, viewPath);

                if (!_views.ContainsKey(viewDef))
                {
                    // Get layout template (e.g. Views/Shared/layout.hbs)
                    string layout = File.ReadAllText(layoutPath);
                    
                    // Get view template (e.g. Views/Home/Index.hbs)
                    string source = File.ReadAllText(viewPath);

                    // Create page (insert view into layout)
                    string page = layout.Replace("{{{body}}}", source);

                    // Compile template
                    var template = Handlebars.Compile(page);

                    _views.Add(viewDef, template);
                    return template;
                }
                else
                {
                    return _views[viewDef];
                }                           
            }
        }
    }

    internal class ViewDefinition : IEquatable<ViewDefinition>
    {
        public string LayoutPath { get; set; }
        public string ViewPath { get; set; }

        public ViewDefinition(string layoutPath, string viewPath)
        {
            if (string.IsNullOrWhiteSpace(layoutPath))
                throw new ArgumentException(nameof(layoutPath) + " is null or whitespace", nameof(layoutPath));
            if (string.IsNullOrWhiteSpace(viewPath))
                throw new ArgumentException(nameof(viewPath) + " is null or whitespace", nameof(viewPath));
        }

        public bool Equals(ViewDefinition other)
        {
            return LayoutPath.Equals(other.LayoutPath, StringComparison.OrdinalIgnoreCase) &&
                ViewPath.Equals(other.ViewPath, StringComparison.OrdinalIgnoreCase);
        }
    }

}
