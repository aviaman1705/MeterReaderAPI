namespace MeterReaderAPI.DTO
{
    public class MonthlyDataDTO
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Date { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int Called { get; set; }
        public int UnCalled { get; set; }
        public double Percentage { get; set; }
    }
}
