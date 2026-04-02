using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models
{
    public class Villa
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Details { get; set; }
        public double Rate { get; set; } // سعر الليلة
        public int Sqft { get; set; } // المساحة
        public int Occupancy { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; } // المميزات

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
