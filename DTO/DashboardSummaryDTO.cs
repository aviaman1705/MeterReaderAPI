namespace MeterReaderAPI.DTO
{
    public class DashboardSummaryDTO
    {
        public int Called { get; set; }
        public int UnCalled { get; set; }
        public int MonthlyCalled { get; set; }
        public int MonthlyUnCalled { get; set; }
    }
}
