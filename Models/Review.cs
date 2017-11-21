using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

namespace yelp.Models
{
    public class Review : BaseEntity
    {
        [Key]
        public int ReviewId { get; set; }
        public int ListingId{ get; set; }
        public Listing Listing { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public int Rating { get; set; }
        public int Helpful { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }

        public Review()
        {
            Helpful = 0;
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }

    public class ReviewViewModel : BaseEntity
    {
        [Required(ErrorMessage = "You must enter a Review")]
        [MinLength(2, ErrorMessage = "Review must be at least ten characters")]
        public string Content { get; set; }

        [Required (ErrorMessage = "You must enter an Image")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "You must enter a Rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 to 5")]
        public int Rating { get; set; }
        public int ListingId { get; set; }
    }
}