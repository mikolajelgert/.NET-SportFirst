using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SportFirst.Models;

namespace SportFirst.Controllers
{
    public class ReservationsController : Controller
    {
        //public IActionResult Calendar() 
        //{
        //    var days = generateTwoWeeksSlots();
        //    return View(days);
        //}

        private List<DaySlot> generateTwoWeeksSlots() 
        {
            List<DaySlot> days = new List<DaySlot>();
            DateTime today = DateTime.Today;

            for (int i = 0; i < 14; i++)
            {
                DateTime date = today.AddDays(i);
                string dayName = CultureInfo.GetCultureInfo("en-US").DateTimeFormat.GetDayName(date.DayOfWeek);
                string fullDate = $"{dayName} {date:dd MMM}";

                var slots = new List<TimeSlot>();
                for (int hour = 7; hour < 20; hour++) 
                {
                    slots.Add(new TimeSlot
                    {
                        Hour = $"{hour:00}:00",
                        IsAvailable = new Random().Next(0, 2) == 1
                    });
                
                }
                days.Add(new DaySlot {Day = fullDate, TimeSlots = slots});
            }
            return days;
        }

    }
}
