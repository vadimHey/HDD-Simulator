namespace HDDSimulator.Model
{
    // Модель запроса для работы с диском
    public class DiskRequest
    {
        public int Id { get; set; }
        public int CylinderNumber { get; set; }
        public DateTime ArrivalTime { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime? CompletionTime { get; set; }
        public required string FileName { get; set; } = string.Empty;
    }
}