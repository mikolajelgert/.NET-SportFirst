using System.ComponentModel.DataAnnotations;

namespace SportFirst.Models
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime AddedOn { get; set; }

        public DateTime ReservationDateTime { get; set; }

        public string SelectedSport { get; set; }
    }
}
