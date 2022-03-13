namespace ControlService.Models
{
    public class Command
    {
        public int Id { get; set; }
        public string CommandText { get; set; }
        public DateTime TimeCreation { get; set; }
        public int UserId { get; set; }
        public int RemoteComputerId { get; set; }
    }
}
