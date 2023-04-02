namespace MeterReaderAPI.DTO
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;

        private int recordsPerPage = 10;
        private readonly int maxRecordsPerPage = 50;

        public int RecrdsPerPage
        {
            get
            {
                return recordsPerPage;
            }
            set
            {
                recordsPerPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value;
            }
        }

        public string SortColumn { get; set; }
      
        public string SortType { get; set; }

        public string Search { get; set; }
    }
}
