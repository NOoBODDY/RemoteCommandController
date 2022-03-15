
using System.ComponentModel.DataAnnotations;

namespace WebServer.ViewModels
{
    public class ModuleViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public string FileType { get; set; }
        public IFormFile File { get; set; }
    }
}
