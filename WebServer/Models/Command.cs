using System.Text.Json.Serialization;
namespace WebServer.Models
{
    public class Command
    {
        public int Id { get; set; }
        public string CommandText { get; set; }
        public DateTime TimeCreation { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int UserId { get; set; }
        public int RemoteComputerId { get; set; }
        [JsonIgnore]
        public RemoteComputer RemoteComputer { get; set; }
    }
}
