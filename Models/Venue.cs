namespace RiyaBhandari_Lab2.Models
{
    public class Venue
    {
        public int VenueId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? ContactEmail { get; set; }


        // Navigation
        public List<MembershipPlan> MembershipPlans { get; set; } = new();
        public List<Booking> Bookings { get; set; } = new();
    }
}
