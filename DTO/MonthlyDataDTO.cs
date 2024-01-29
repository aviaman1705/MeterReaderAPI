namespace MeterReaderAPI.DTO
{
    public class MonthlyDataDTO
    {
        public string Date { get; set; }
        public int Called { get; set; }
        public int UnCalled { get; set; }
        public double Percentage { get; set; }
    }
}
