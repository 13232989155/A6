using Admin.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.ViewComponents
{
    public class ThisAdminViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            AdminEntity adminEntity = HttpContext.Session.Get<AdminEntity>("admin");

            return View(adminEntity);
        }

    }
}
