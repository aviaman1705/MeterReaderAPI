using MeterReaderAPI.Entities;

namespace MeterReaderAPI.DTO
{
    public class DashboardDTO
    {
        public DashboardDTO()
        {
            DashboardSummary = new DashboardSummaryDTO();
            MonthlyData = new List<MonthlyDataDTO>();
        }
        public DashboardSummaryDTO DashboardSummary { get; set; }
        public List<MonthlyDataDTO> MonthlyData { get; set; }
    }
}
