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
        public ProjectContext _context;
        public HomeController(ProjectContext context)
        {
            _context = context;
        }
        public bool CheckLoggedIn()
        {
            int? id = HttpContext.Session.GetInt32("UserId");
            User LoggedIn = _context.User.SingleOrDefault(user => user.UserId == id);
            ViewBag.userName = HttpContext.Session.GetString("UserName");
            ViewBag.User = LoggedIn;
            if(ViewBag.User != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpGet]
        [Route("")]
        public IActionResult Dashboard()
        {
            if(!CheckLoggedIn())
            { 
                return RedirectToAction("Index", "LogReg");
            } else {
                return View(); 
            }
        }
    }
}