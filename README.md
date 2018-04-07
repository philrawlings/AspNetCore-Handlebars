# AspNetCore-Handlebars

A handlebars view engine for ASP.NET Core
Source code contains the View Engine library and an exmaple ASP.NET Core 2.0 Web project
Uses the Handlebars.net library for view rendering: https://github.com/rexm/Handlebars.Net

## Usage

*A Nuget package has not been created yet - this is planned*

* Reference the HandlebarsViewEngine library from your ASP.NET core project

* Add the following using statement to the top of Startup.cs:
```C#
using HandlebarsViewEngine;
```

* Modify Startup.cs as follows:

```C#
public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc()
                .AddHandlebars();

        // If you want to override the default layout location
        //services.Configure<HandlebarsViewEngineOptions>(options =>
        //{
        //    options.DefaultLayout = "Views/Shared/layout.hbs";
        //});
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseBrowserLink();
            app.UseDeveloperExceptionPage();
        }

        // To allow access static files, such as .css and .js file
        app.UseStaticFiles();

        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        });

        app.Run(async (context) =>
        {
            await context.Response.WriteAsync("Could not match route");
        });
    }
}
```
* Create the following folder structure in your project (files will be created in later steps, this just shows what the folders will look like when populated with files)
```
Controllers
-- {controller-name}Controller.cs
Views
-- {controller-name}
---- {action-name}.hbs
---- Partials [optional]
------ {controller-name}-{partial-name}.hbs [optional]
-- Shared
---- layout.hbs
---- Partials
------ {partial-name}.hbs
```

* Create a layout template - layout.hbs. For example:
```HTML
<!DOCTYPE html>
<html>

<head>
    <title>Handlebars Example</title>   
    <link href="./style.css" rel="stylesheet">
</head>

<body class="app">
    <div>
        <div id="headerContent">
            {{> header}} <!-- Optional partial view -->
        </div>
        <div id="mainContent">
            {{{body}}} <!-- View content will be here -->
        </div>
    </div>
</body>

</html>
```

* Create controllers as you would in vanilla ASP.NET Core, passing a model to the view if it is required in the template. For example:

```C#
public class PeopleController : Controller
{
    public IActionResult Index()
    {
        var people = new List<Person>();

        people.Add(new Person
        {
            Name = "Amit Saluja",
            Position = "Business Analyst",
            Office = "Bangalore"
        });
        people.Add(new Person
        {
            Name = "Sofia Souza",
            Position = "Marketing Manager",
            Office = "São Paulo"
        });

        return View(people);
    }
}
```
* Create Views for your actions, for example:

```HTML
<table cellpadding="10">
    <thead>
        <tr>
            <th>Name</th>
            <th>Position</th>
            <th>Office</th>
        </tr>
    </thead>
    <tbody>
        {{#each this}}
        <tr>
            <td>{{Name}}</td>
            <td>{{Position}}</td>
            <td>{{Office}}</td>
        </tr>
        {{/each}}
    </tbody>
</table>
```


## Limitations

* All partial views must be uniquely named, regarless of which folder they are in. It is recommended that shared partials are stored in Views/Shared/Partials. More specific partials can be stored in Views/{Controller-Name}/Partials but should be named {controller-name}-{partial-name}.hbs. This is due to the fact that handlebars.net loads all partials globally, keyed by a name. Therefore, the view engine loads all partials at startup and throws an exception if there are any duplicates.

* Only data passed to a view as a model is currently supported. Therefore, items added to the ViewBag are not accessible.

