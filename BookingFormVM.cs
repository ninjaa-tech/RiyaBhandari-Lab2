using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using RiyaBhandari_Lab2.Models;

namespace RiyaBhandari_Lab2.ViewModels
{
    /// <summary>
    /// ViewModel for Create/Edit Booking forms.
    /// Combines a Booking object with dropdown & checkbox data sources.
    /// </summary>
    public class BookingFormVM
    {
        public Booking Booking { get; set; } = new();

        // List of AddOn IDs chosen by user
        public List<int> SelectedAddOnIds { get; set; } = new();

        // Dropdown options
        public IEnumerable<SelectListItem> VenueOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> MemberOptions { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> AddOnOptions { get; set; } = new List<SelectListItem>();
    }
}
