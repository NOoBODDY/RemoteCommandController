namespace WebServer.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public User Author { get; set; }

        public List<RemoteComputer> RemoteComputers { get; set; }

        public Module()
        {
            RemoteComputers = new List<RemoteComputer>();
        }
    }
}
