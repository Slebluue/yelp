using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

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

    }
}