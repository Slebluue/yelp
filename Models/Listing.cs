using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace yelp.Models
{
    public class Listing : BaseEntity
    {
        [Key]
        public int ListingId { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public int Phone { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        List<Review> Reviews {get; set;}
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }

        public Listing(){
            Reviews = new List<Review>();
        }

    }
}