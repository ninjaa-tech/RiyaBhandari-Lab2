namespace RiyaBhandari_Lab2.Models
{
    public class BookingAddOn
    {
        public int BookingId { get; set; }
        public Booking? Booking { get; set; }


        public int AddOnId { get; set; }
        public AddOn? AddOn { get; set; }
    }
}
