namespace RiyaBhandari_Lab2.Models
{
    public class AddOn
    {
        public int AddOnId { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., Equipment rental, Coaching, Refreshments
        public decimal Cost { get; set; }


        public List<BookingAddOn> BookingLinks { get; set; } = new();
    }
}
