using System.Text.Json.Serialization;

namespace ControlService.Models
{
    public class Command
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("commandText")]
        public string CommandText { get; set; }
        [JsonPropertyName("timeCreation")]
        public DateTime TimeCreation { get; set; }
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("remoteComputerId")]
        public int RemoteComputerId { get; set; }
    }
}
