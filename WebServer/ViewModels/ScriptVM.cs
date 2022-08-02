using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
namespace WebServer.ViewModels
{
    public class ScriptVM
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано имя файла")]
        public string FileName { get; set; }
        [Required(ErrorMessage = "Текст файла пуст")]
        public string FileText { get; set; }
        [Required(ErrorMessage = "Не указано путь файла")]
        public string FilePath { get; set; }

    }
}
