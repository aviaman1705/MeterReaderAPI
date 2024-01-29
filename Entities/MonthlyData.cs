namespace MeterReaderAPI.Entities
{
    public class MonthlyData
    {
        public string Date { get; set; }
        public int Called { get; set; }
        public int UnCalled { get; set; }
        public double Percentage { get; set; }
    }
}
