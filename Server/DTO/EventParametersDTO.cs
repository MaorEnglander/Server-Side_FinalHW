namespace MyServer.DTO
{
    public class EventParametersDTO
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int MaxRegistrations { get; set; }

        public string Location { get; set; } = null!;
    }
}
