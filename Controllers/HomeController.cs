using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using yelp.Models;

namespace yelp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("UserName") == null)
            { 
                return RedirectToAction("Index", "LogReg");
            } else {
                ViewBag.userName = HttpContext.Session.GetString("UserName");
                return View(); 
            }
        }
    }
}