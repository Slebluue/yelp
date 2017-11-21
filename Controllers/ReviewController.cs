using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using yelp.Models;

using Microsoft.AspNetCore.Hosting; //For IHostingEnvironmeny (getting file paths)
using System.IO; //For Path method

namespace yelp.Controllers
{
    public class ReviewController : Controller
    {
        public ProjectContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public ReviewController(ProjectContext context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
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
        [Route("reviews")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Index", "LogReg");
            }
            else
            {
                ViewBag.userName = HttpContext.Session.GetString("UserName");
                return View();
            }
        }

        [HttpGet]
        [Route("reviews/{list_id}")]
        public IActionResult DisplayReviews(int list_id)
        {
            CheckLoggedIn();
            if(!_context.Listings.Any(l => l.ListingId == list_id))
            {
                return RedirectToAction("Index", "Listing");
            }
            else
            {
                ViewBag.userName = HttpContext.Session.GetString("UserName");
                List<Review> selectedReviews = _context.Reviews.Where(review => review.ListingId == list_id)
                    .OrderByDescending(review => review.Created_At).ToList();

                int ratingSum = selectedReviews.Sum(reviews => reviews.Rating);
                if(ratingSum != 0)
                {
                    ratingSum = ratingSum / selectedReviews.Count;
                }
                
                ViewBag.Id = list_id;
                ViewBag.rating = ratingSum;
                ViewBag.reviews = selectedReviews;
                return View();
            }
        }

        [HttpPost]
        [Route("createReview")]
        public async Task<IActionResult> CreateReview(ReviewViewModel submittedReview)
        {
            //This isn't just a random request with no one logged in
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Index", "LogReg");
            }
            else
            {
                int currentUserId = (int)HttpContext.Session.GetInt32("UserId"); 
                User currentUser = _context.User.SingleOrDefault(user => user.UserId == currentUserId);
                if(currentUser != null)
                {
                    //A user was actually able to be found with that user ID in session
                    if(ModelState.IsValid)
                    {
                        //The SubmittedReview model is valid
                        Listing selectedListing = _context.Listings.SingleOrDefault(listing => listing.ListingId == submittedReview.ListingId);

                        Review newReview = new Review
                        {
                            Listing = selectedListing,
                            Content = submittedReview.Content,
                            Rating = submittedReview.Rating,
                            User = currentUser
                        };
                        var uploadDestination = Path.Combine(_hostingEnvironment.WebRootPath, "uploaded_images");
                        //Validate that the IFormFile is populated
                        if (submittedReview.Image.Length > 0)
                        {
                            var filepath = Path.Combine(uploadDestination, submittedReview.Image.FileName);
                            using(var filestream = new FileStream(filepath, FileMode.Create))
                            {
                                await submittedReview.Image.CopyToAsync(filestream);
                                newReview.Picture = "/uploaded_images/" + submittedReview.Image.FileName;
                            }
                        }
                        _context.Reviews.Add(newReview);
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction("DisplayReviews", new {list_id = submittedReview.ListingId});
            }
        }
        
        [HttpGet]
        [Route("helpful/{review_id}")]
        public int HelpfulReview(int review_id)
        {
            Review selectedReview = _context.Reviews.SingleOrDefault(review => review.ReviewId == review_id);
            if (selectedReview != null)
            {
                selectedReview.Helpful += 1;
            }
            return selectedReview.Helpful;
        }
        
    }
}