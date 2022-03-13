namespace WebServer.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string message { get; set; }
        public DateTime DateTime { get; set; }

        public int RemoteComputerId { get; set; }
        public RemoteComputer RemoteComputer { get; set; }
    }
}
