namespace WebServer.Models
{
    public class UserParamsForRemote
    {
        public int Id { get; set; }
        public string? ComputerName { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        public int RemoteComputerId { get; set; }
        public RemoteComputer RemoteComputer { get; set; }
    }
}
