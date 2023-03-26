namespace MeterReaderAPI.DTO
{
    public class PaginationDTO
    {
        public int PageIndex { get; set; } = 1;

        private int pageSize = 5;

        private readonly int maxRecordsPerPage = 50;       
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = (value > maxRecordsPerPage) ? maxRecordsPerPage : value;
            }
        }
    }
}

