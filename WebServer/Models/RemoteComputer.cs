namespace WebServer.Models
{
    public class RemoteComputer
    {
        public int Id { get; set; }
        public string GUID { get; set; }
        public DateTime LastConnection { get; set; }

        public List<UserParamsForRemote?> UserParamsForRemotes { get; set; }
        public List<Message> Messages { get; set; }
        public List<Module> Modules { get; set; }
        public List<Command> Commands { get; set; }

        public RemoteComputer()
        {
            UserParamsForRemotes = new List<UserParamsForRemote?>();
            Messages = new List<Message>();
            Modules = new List<Module>();
            Commands = new List<Command>();
        }

        
    }
}
