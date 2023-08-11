namespace MeterReaderAPI.Entities
{
    public class Dashboard
    {
        public Dashboard()
        {
            DashboardSummary = new DashboardSummary();
            MonthlyData = new List<MonthlyData>();
        }
        public DashboardSummary DashboardSummary { get; set; }
        public List<MonthlyData> MonthlyData { get; set; }
    }
}
