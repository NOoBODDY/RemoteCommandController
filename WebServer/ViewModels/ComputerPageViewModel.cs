using WebServer.Models;

namespace WebServer.ViewModels
{
    public class ComputerPageViewModel
    {
        public int Id { get; set; }
        public string ComputerName { get; set;}
        public string Command { get; set; }
        public DateTime LastConnection { get; set; }
        public int UserId { get; set; }
        public List<Modul> Moduls { get; set; }
        public List<Message> Messages { get; set; }
    }
}
