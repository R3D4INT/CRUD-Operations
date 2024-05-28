using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{

    public class UserController : Controller
    {
        private readonly string apiBaseUrl = "http://localhost:5000/api/users";
        public ActionResult Index()
        {
            return View("User");
        }
    }
}