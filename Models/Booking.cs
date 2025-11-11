namespace RiyaBhandari_Lab2.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public DateOnly Date { get; set; }
        public string TimeSlot { get; set; } = string.Empty; // e.g., "10:00–12:00"
        public decimal TotalPrice { get; set; }


        public int VenueId { get; set; }
        public Venue? Venue { get; set; }


        public int? MemberId { get; set; }
        public Member? Member { get; set; }


        public List<BookingAddOn> AddOns { get; set; } = new();
    }
}
