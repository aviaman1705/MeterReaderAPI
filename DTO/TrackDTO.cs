namespace MeterReaderAPI.DTO
{
    public class TrackDTO
    {
        public int Id { get; set; }

        public int Called { get; set; }

        public int UnCalled { get; set; }

        public string Desc { get; set; }

        public string Date { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
