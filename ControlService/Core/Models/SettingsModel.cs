namespace ControlService.Core.Models
{
    public class SettingsModel
    {
        public string Guid { get; set; }
        public List<string> ExternalModules { get; set; }
        public int Delay { get; set; }
    }
}
