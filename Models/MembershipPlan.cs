using System.Diagnostics.Metrics;

namespace RiyaBhandari_Lab2.Models
{
    public class MembershipPlan
    {
        public int MembershipPlanId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Perks { get; set; } = string.Empty; // short description
        public decimal DiscountPercent { get; set; }


        // Navigation
        public int VenueId { get; set; }
        public Venue? Venue { get; set; }
        public List<Member> Members { get; set; } = new();
    }
}
