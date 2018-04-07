using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HandlebarsViewEngine.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HandlebarsViewEngine.Web.Controllers
{
    public class PeopleController : Controller
    {
        public IActionResult Index()
        {
            var people = new List<Person>();

            people.Add(new Person
            {
                Name = "Jane Doe",
                Position = "Chief Operating Officer",
                Office = "London"
            });
            people.Add(new Person
            {
                Name = "John Smith",
                Position = "Systems Architect",
                Office = "New York"
            });
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
}