using System;
using System.Collections.Generic;
using System.Linq;
using RiyaBhandari_Lab2.Models;

namespace RiyaBhandari_Lab2.Data
{
    /// <summary>
    /// Simple in-memory data store used to satisfy Lab 2 without a database.
    /// Includes seed data and small CRUD helpers for Member & Booking.
    /// </summary>
    public static class InMemoryData
    {
        public static List<Venue> Venues { get; private set; } = new();
        public static List<Member> Members { get; private set; } = new();
        public static List<MembershipPlan> Plans { get; private set; } = new();
        public static List<AddOn> AddOns { get; private set; } = new();
        public static List<Booking> Bookings { get; private set; } = new();

        private static bool _seeded = false;

        public static void Seed()
        {
            if (_seeded) return;

            Venues = new()
            {
                new Venue { VenueId = 1, Name = "City Sports Arena", Location = "Melbourne CBD", Capacity = 200, ContactEmail = "contact@cityarena.au" },
                new Venue { VenueId = 2, Name = "Westside Courts",   Location = "Footscray",     Capacity = 120, ContactEmail = "hello@westside.au"  }
            };

            Plans = new()
            {
                new MembershipPlan { MembershipPlanId = 1, Name = "Silver",        Price = 29.99m, Perks = "5% discount, standard slots", DiscountPercent = 5,  VenueId = 1 },
                new MembershipPlan { MembershipPlanId = 2, Name = "Gold",          Price = 59.99m, Perks = "10% discount, priority booking", DiscountPercent = 10, VenueId = 1 },
                new MembershipPlan { MembershipPlanId = 3, Name = "Student Saver", Price = 19.99m, Perks = "Budget access", DiscountPercent = 0, VenueId = 2 }
            };

            Venues[0].MembershipPlans.AddRange(Plans.Where(p => p.VenueId == 1));
            Venues[1].MembershipPlans.AddRange(Plans.Where(p => p.VenueId == 2));

            Members = new()
            {
                new Member { MemberId = 1, FullName = "Ava Nguyen",  Email = "ava@yahoo.com",  Phone = "0400 111 222", MembershipPlanId = 2 },
                new Member { MemberId = 2, FullName = "Liam Patel",  Email = "liam@google.com", Phone = "0400 333 444", MembershipPlanId = 1 }
            };

            AddOns = new()
            {
                new AddOn { AddOnId = 1, Name = "Equipment Rental", Cost = 25m },
                new AddOn { AddOnId = 2, Name = "Coaching (1 hr)",  Cost = 60m },
                new AddOn { AddOnId = 3, Name = "Refreshments",     Cost = 15m }
            };

            Bookings = new()
            {
                new Booking
                {
                    BookingId = 1,
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                    TimeSlot = "10:00–12:00",
                    VenueId = 1,
                    MemberId = 1,
                    TotalPrice = 120m,
                    AddOns = new List<BookingAddOn> { new BookingAddOn { BookingId = 1, AddOnId = 1 } }
                },
                new Booking
                {
                    BookingId = 2,
                    Date = DateOnly.FromDateTime(DateTime.Today.AddDays(2)),
                    TimeSlot = "14:00–16:00",
                    VenueId = 2,
                    MemberId = 2,
                    TotalPrice = 90m
                }
            };

            // Reverse navigation
            foreach (var b in Bookings)
            {
                b.Venue = Venues.First(v => v.VenueId == b.VenueId);
                b.Member = Members.First(m => m.MemberId == b.MemberId);
                b.Venue.Bookings.Add(b);
                b.Member.Bookings.Add(b);
            }

            _seeded = true;
        }

        // ---------- Utility finders ----------
        public static Venue? FindVenue(int id) => Venues.FirstOrDefault(v => v.VenueId == id);
        public static Member? FindMember(int id) => Members.FirstOrDefault(m => m.MemberId == id);
        public static MembershipPlan? FindPlan(int id) => Plans.FirstOrDefault(p => p.MembershipPlanId == id);
        public static Booking? FindBooking(int id) => Bookings.FirstOrDefault(b => b.BookingId == id);

        // ---------- Member CRUD ----------
        public static Member AddMember(Member m)
        {
            m.MemberId = (Members.Count == 0 ? 1 : Members.Max(x => x.MemberId) + 1);
            Members.Add(m);
            return m;
        }

        public static bool UpdateMember(Member updated)
        {
            var m = FindMember(updated.MemberId);
            if (m == null) return false;
            m.FullName = updated.FullName;
            m.Email = updated.Email;
            m.Phone = updated.Phone;
            m.MembershipPlanId = updated.MembershipPlanId;
            return true;
        }

        public static bool DeleteMember(int id)
        {
            var m = FindMember(id);
            if (m == null) return false;

            // Remove their bookings too (simple cascade for demo)
            foreach (var b in Bookings.Where(b => b.MemberId == id).ToList())
                DeleteBooking(b.BookingId);

            Members.Remove(m);
            return true;
        }

        // ---------- Booking CRUD ----------
        public static Booking AddBooking(Booking b)
        {
            b.BookingId = (Bookings.Count == 0 ? 1 : Bookings.Max(x => x.BookingId) + 1);
            // link nav
            b.Venue = FindVenue(b.VenueId);
            b.Member = b.MemberId.HasValue ? FindMember(b.MemberId.Value) : null;
            b.Venue?.Bookings.Add(b);
            b.Member?.Bookings.Add(b);
            Bookings.Add(b);
            return b;
        }

        public static bool UpdateBooking(Booking updated)
        {
            var b = FindBooking(updated.BookingId);
            if (b == null) return false;

            // unlink old nav
            b.Venue?.Bookings.Remove(b);
            b.Member?.Bookings.Remove(b);

            b.Date = updated.Date;
            b.TimeSlot = updated.TimeSlot;
            b.TotalPrice = updated.TotalPrice;
            b.VenueId = updated.VenueId;
            b.MemberId = updated.MemberId;

            // relink
            b.Venue = FindVenue(b.VenueId);
            b.Member = b.MemberId.HasValue ? FindMember(b.MemberId.Value) : null;
            b.Venue?.Bookings.Add(b);
            b.Member?.Bookings.Add(b);

            // update add-ons
            b.AddOns = updated.AddOns ?? new List<BookingAddOn>();
            return true;
        }

        public static bool DeleteBooking(int id)
        {
            var b = FindBooking(id);
            if (b == null) return false;
            b.Venue?.Bookings.Remove(b);
            b.Member?.Bookings.Remove(b);
            Bookings.Remove(b);
            return true;
        }
    }
}
