namespace WebServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
        public List<UserParamsForRemote> UserParamsForRemotes { get; set; }
        public List<Command> Commands { get; set; }
        public List<Module> Moduls { get; set; }

        public User()
        {
            UserParamsForRemotes = new List<UserParamsForRemote>();
            Commands = new List<Command>();
        }

    }
}
