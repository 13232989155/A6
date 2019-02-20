using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using Admin.Base;

namespace Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (!IsLogin())
            {
                return RedirectToAction(controllerName: "Login", actionName: "Login");
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 隐私条款
        /// </summary>
        /// <returns></returns>
        public IActionResult Notice()
        {
            return View();
        }

    }
}
