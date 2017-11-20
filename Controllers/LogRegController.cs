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
    public class LogRegController : Controller
    {
        public ProjectContext _context;
        public LogRegController(ProjectContext context)
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
        [Route("index")]
        [ImportModelState]
        public IActionResult Index()
        {
            CheckLoggedIn();
            if (TempData["errors"] != null)
            {
                ViewBag.errors = ModelState;
            }
            if (TempData["login_errors"] != null)
            {
                ViewBag.login_errors = ModelState;
            }
            return View();
        }

        [HttpPost]
        [Route("register")]
        [ExportModelState]
        public IActionResult Register(LoginRegViewModel registeringUser)
        {
            if(TryValidateModel(registeringUser.RegisterVM))
            {
                //If the model validation is sucessful check to see if the email 
                //is already in the database. "SingleOrDefault" returns null if it can't find it
                User existingUser = _context.User.SingleOrDefault(user => user.Email == registeringUser.RegisterVM.Email.ToLower());
                if(existingUser == null)
                {
                    //Create a new User object to be insterted into the database
                    User newUser = new User
                    {
                        FirstName = registeringUser.RegisterVM.FirstName,
                        LastName = registeringUser.RegisterVM.LastName,
                        Email = registeringUser.RegisterVM.Email.ToLower(),
                        Created_At = DateTime.Now,
                        Updated_At = DateTime.Now
                    };
                    //Create a password hasher object and insert the user's hashed password
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    newUser.Password = hasher.HashPassword(newUser, registeringUser.RegisterVM.RegPassword);
                    //Try and insert the new user into the database
                    try
                    {
                        _context.User.Add(newUser);
                        _context.SaveChanges();
                        HttpContext.Session.SetString("UserName", newUser.FirstName);
                        HttpContext.Session.SetInt32("UserId", newUser.UserId);
                        return RedirectToAction("Dashboard", "Home");
                    }
                    catch (System.Exception)
                    {
                        //There was an error with inserting into the database
                        TempData["errors"] = true;
                        return RedirectToAction("Index");
                    }
                } else {
                    //Input email is already in the database
                    string key = "Email";
                    string errorMessage = "This email address already exists. Please select another or login.";
                    ModelState.AddModelError(key, errorMessage);
                    TempData["errors"] = true;
                    return RedirectToAction("Index");
                }
            } else {
                //The model could not be validated. Errors in form
                TempData["errors"] = true;
                return RedirectToAction("Index");
            }
        }
        
        [HttpPost]
        [Route("login")]
        [ExportModelState]
        public IActionResult Login(LoginRegViewModel loggingInUser)
        {
            if(TryValidateModel(loggingInUser.LoginVM))
            {
                //If the Model is validation is sucessful try finding the user
                User existingUser = _context.User.SingleOrDefault(user => user.Email == loggingInUser.LoginVM.LoginEmail.ToLower());
                if (existingUser != null)
                {
                    //Create a password hasher object and compare the user's hashed passwords
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    //Match the input password with our database (a match == 1, 0 == error)
                    if(hasher.VerifyHashedPassword(existingUser, existingUser.Password, loggingInUser.LoginVM.LoginPassword) != 0)
                    {
                        //The passwords succesfully matched
                        HttpContext.Session.SetString("UserName", existingUser.FirstName);
                        HttpContext.Session.SetInt32("UserId", existingUser.UserId);
                        return RedirectToAction("dashboard", "Home");
                    } else {
                        //The passwords did not match
                        string key = "LoginEmail";
                        string errorMessage = "Email address or log in did not work";
                        ModelState.AddModelError(key, errorMessage);
                        TempData["login_errors"] = true;
                        return RedirectToAction("Index");
                    }
                } else {
                    //The user couldn't be found in the database
                    string key = "LoginEmail";
                    string errorMessage = "Email address or log in did not work";
                    ModelState.AddModelError(key, errorMessage);
                    TempData["login_errors"] = true;
                    return RedirectToAction("Index");
                }
            } else {
                //The model could not be validated. Errors in form.
                TempData["login_errors"] = true;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}