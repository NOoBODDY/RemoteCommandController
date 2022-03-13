namespace WebServer.ViewModels
{
    public class ComputerPageViewModel
    {
        public int Id { get; set; }
        public string ComputerName { get; set;}
        public string Command { get; set; }
        public DateTime LastConnection { get; set; }
        public int UserId { get; set; }
    }
}
