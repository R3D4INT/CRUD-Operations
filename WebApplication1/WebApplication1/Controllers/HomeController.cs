using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using back;
using back.Models;
using back.Models.enums;


namespace WebApplication1.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly IUserRepository _repository;

        public HomeController(IUserRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var user = new User()
            {
                Name = "John",
                Surname = "Doe",
                Age = 30,
                Email = "john.doe@example.com",
                Address = "123 Main St, Anytown, USA",
                Gender = Gender.Male
            };
            await _repository.AddAsync(user);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}