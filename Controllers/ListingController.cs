using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using yelp.Models;
using yelp.ActionFilters; //For Error Messages

namespace yelp.Controllers
{
    public class ListingController : Controller
    {
        //Current datetime
        public DateTime now()
        {
            DateTime now = DateTime.Now;
            return now;
        }
        public bool CheckLoggedIn()
        {
            int? id = HttpContext.Session.GetInt32("UserId");
            User LoggedIn = _context.User.SingleOrDefault(user => user.UserId == id);
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
        public ProjectContext _context;
        public ListingController(ProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/listings")]
        public IActionResult Index(string search, string category)
        {
            if(!CheckLoggedIn())
            { 
                return RedirectToAction("Index", "LogReg");
            } 
            if(search != null)
            {
                if(category == "Name"){
                    List<Listing> NameSearch = _context.Listings.Where(l => l.Name.ToLower().Contains(search.ToLower())).ToList();
                    return View(NameSearch);
                }
                if(category == "City")
                {
                    List<Listing> CitySearch = _context.Listings.Where(l => l.Address.ToLower().Contains(search.ToLower())).ToList();
                    return View(CitySearch);
                }
                if(category == "Category"){
                    List<Listing> CategorySearch = _context.Listings.Where(l => l.Category.ToLower() == search.ToLower()).ToList();
                    return View(CategorySearch);
                }
            }
            List<Listing> model = _context.Listings.ToList();
            return View(model);
        }

        [HttpGet]
        [Route("/listings/{id}")]
        public IActionResult Listing(int id)
        {
            if(!CheckLoggedIn())
            { 
                return RedirectToAction("Index", "LogReg");
            } 
            Listing model = _context.Listings.SingleOrDefault(l => l.ListingId == id);
            return View(model);
        }

        [HttpPost]
        [Route("/search")]
        public IActionResult Search(string searchContent, string categoryContent)
        {
            return RedirectToAction("Index", new {search = searchContent, category = categoryContent});
        }

        // THESE SHOULD NOT BE ACCESSIBLE FOR USERS JUST FOR US TO CREATE INTO DATABASE
        [HttpGet]
        [Route("/create")]
        public IActionResult Create(string Name, string Picture, int Phone, string Category, string Description, string Address)
        {
            CheckLoggedIn();
            return View("Create");
        }

        [HttpPost]
        [Route("/create")]
        public IActionResult CreateListing(string Name, string Picture, int Phone, string Category, string Description, string Address)
        {
            Listing NewListing = new Listing(){
                Name = Name,
                Picture = Picture,
                Phone = Phone,
                Category = Category,
                Description = Description,
                Address = Address,
                Created_At = now(),
                Updated_At = now(),
            };
            _context.Listings.Add(NewListing);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}