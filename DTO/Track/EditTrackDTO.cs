namespace MeterReaderAPI.DTO.Track
{
    public class EditTrackDTO
    {
        public int Id { get; set; }

        public int Called { get; set; }

        public int UnCalled { get; set; }

        public string Desc { get; set; }

        public DateTime Date { get; set; }

        public DateTime CreatedDate { get; set; }

        public int notebookId { get; set; }       
    }
}
