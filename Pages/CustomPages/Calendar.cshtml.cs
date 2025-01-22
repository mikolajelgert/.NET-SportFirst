using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportFirst.Models;

namespace SportFirst.Pages.CustomPages
{

    public class CalendarModel : PageModel
    {
		public List<SelectListItem> SportOptions { get; set; }

        [BindProperty]
        public Reservation Reservation { get; set; }

        [BindProperty]
        public string SelectedSport { get; set; } = "tennis";

        [BindProperty]
        public int CurrentWeek { get; set; } = 0;

        public int CurrentHour { get; set; }

        private static Dictionary<string, List<DaySlot>> SportReservations { get; set; } = new();
        public List<DaySlot> Days => SportReservations.ContainsKey(SelectedSport)
            ? SportReservations[SelectedSport].Skip(CurrentWeek * 7).Take(7).ToList()
            : GenerateCalendarSlots().Take(7).ToList();

        public void OnGet()
        {
            if (string.IsNullOrEmpty(SelectedSport))
            {
                SelectedSport = "tennis";
            }

            InitializeSportOptions();

            if (!SportReservations.ContainsKey(SelectedSport))
            {
                SportReservations[SelectedSport] = GenerateCalendarSlots();
            }
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(SelectedSport))
            {
                SelectedSport = "tennis";
            }

            InitializeSportOptions();

            if (!SportReservations.ContainsKey(SelectedSport))
            {
                SportReservations[SelectedSport] = GenerateCalendarSlots();
            }

            CurrentWeek = 0;
            Reservation = null;

            return Page();
        }


        private void LoadData()
        {
            CurrentHour = DateTime.Now.Hour;
            InitializeSportOptions();

            if (!SportReservations.ContainsKey(SelectedSport))
                SportReservations[SelectedSport] = GenerateCalendarSlots();
        }

        public IActionResult OnPostToggleWeek(int currentWeek)
        {
            CurrentWeek = currentWeek;
            Reservation = null;
            LoadData();
            return Page();
        }

        private void InitializeSportOptions()
        {
            SportOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "tennis", Text = "Tennis", Selected = SelectedSport == "tennis" },
                new SelectListItem { Value = "squash", Text = "Squash", Selected = SelectedSport == "squash" },
                new SelectListItem { Value = "table tennis", Text = "Table Tennis", Selected = SelectedSport == "tableTennis" }
            };
        }

        private List<DaySlot> GenerateCalendarSlots()
        {
            List<DaySlot> days = new();
            DateTime today = DateTime.Today;
            DateTime now = DateTime.Now;

            for (int i = 0; i < 14; i++)
            {

                string displayDate = (i == 0) ? "TODAY" : today.AddDays(i).ToString("ddd, dd MMM", new CultureInfo("en-US")); ;

                var slots = new List<TimeSlot>();
                for (int hour = 10; hour < 20; hour++)
                {
                    bool isPast = (i == 0 && (hour < now.Hour || (hour == now.Hour && now.Minute > 0)));

                    slots.Add(new TimeSlot
                    {
                        Hour = $"{hour:00}:00",
                        IsAvailable = isPast ? false : new Random().Next(0, 2) == 1,
                        IsPast = isPast
                    });

                }
                days.Add(new DaySlot { Day = displayDate, TimeSlots = slots });
            }
            return days;
        }

        public IActionResult OnPostOpenReservation(string SelectedDateTime, string SelectedSport)
        {
            if (!string.IsNullOrEmpty(SelectedSport))
            {
                this.SelectedSport = SelectedSport;
            }

            if (!string.IsNullOrEmpty(SelectedDateTime))
            {
                try
                {
                    DateTime parsedDateTime = DateTime.ParseExact(
                        SelectedDateTime, "ddd, dd MMM yyyy HH:mm",
                        new System.Globalization.CultureInfo("en-US")
                    );

                    Reservation = new Reservation
                    {
                        ReservationDateTime = parsedDateTime,
                        SelectedSport = SelectedSport
                    };
                }
                catch (FormatException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ERROR] Date Parsing Failed: {ex.Message}");
                    Reservation = null;
                }
            }

            InitializeSportOptions();
            return Page();
        }

        public IActionResult OnPostCloseReservation(string SelectedSport)
        {
            if (!string.IsNullOrEmpty(SelectedSport))
            {
                this.SelectedSport = SelectedSport;
            }

            Reservation = null;
            InitializeSportOptions();

            return Page();
        }
    }
}
