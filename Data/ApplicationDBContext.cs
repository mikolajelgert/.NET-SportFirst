using Microsoft.EntityFrameworkCore;
using SportFirst.Models;

namespace SportFirst.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>().HasData(
                new Reservation { Id = 1, Name = "Max", Surname = "Hensonn", Email = "henso@gmail.com", SelectedSport = "Tennis", ReservationDateTime = new DateTime(2024, 12, 29, 10, 0, 0) },
                new Reservation { Id = 2, Name = "Tom", Surname = "Yeat", Email = "yeats@gmail.com", SelectedSport = "Squash", ReservationDateTime = new DateTime(2024, 12, 29, 10, 0, 0) }
            );
        }
    }
}
