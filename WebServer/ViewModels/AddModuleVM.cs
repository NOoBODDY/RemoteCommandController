using WebServer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace WebServer.ViewModels
{
    public class AddModuleVM
    {
        [ValidateNever]
        public int ComputerId { get; set; }
        [ValidateNever]
        public SelectList Modules { get; set; }
        [Required(ErrorMessage = "Не указан модуль")]
        public int SelectedModuleId { get; set; }
    }
}
