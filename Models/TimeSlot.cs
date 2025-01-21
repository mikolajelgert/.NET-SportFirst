namespace SportFirst.Models
{
    public class TimeSlot
    {
        public string Hour { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsPast { get; set; }
    }
}
