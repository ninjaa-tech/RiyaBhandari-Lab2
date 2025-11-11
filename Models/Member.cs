using System.ComponentModel.DataAnnotations;

namespace RiyaBhandari_Lab2.Models
{
    public class Member
    {
        public int MemberId { get; set; }

        [Required, StringLength(60)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        public int? MembershipPlanId { get; set; }
        public MembershipPlan? MembershipPlan { get; set; }

        public List<Booking> Bookings { get; set; } = new();
    }
}
