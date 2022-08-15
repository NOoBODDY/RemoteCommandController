using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebServer.Repositories;
using WebServer.Services;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {

        private readonly ILogger<DownloadController> _logger;
        private readonly IModuleService _moduleService;
        IWebHostEnvironment _appEnvironment;

        public DownloadController(ILogger<DownloadController> logger, IModuleService moduleService, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _moduleService = moduleService;
            _appEnvironment = appEnvironment;
        }

        [HttpGet("file")]
        public async Task<VirtualFileResult> Get([Required] string name)
        {
            Module module = await _moduleService.GetModuleByName(name);
            if (module != null)
            {
                return File("~" + module.FilePath, "application/octet-stream", module.Name + "." + module.FileType);
            }
            return null;
        }

    }
}
