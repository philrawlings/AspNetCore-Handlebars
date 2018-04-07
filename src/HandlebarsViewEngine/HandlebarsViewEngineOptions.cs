using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HandlebarsViewEngine
{
    public class HandlebarsViewEngineOptions
    {
        public IList<string> ViewLocationFormats { get; } = new List<string>();
        public string DefaultLayout { get; set; }
    }
}
