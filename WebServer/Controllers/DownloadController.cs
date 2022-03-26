using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {

        private readonly ILogger<DownloadController> _logger;
        DataBaseContext _dbContext;
        IWebHostEnvironment _appEnvironment;

        public DownloadController(ILogger<DownloadController> logger, DataBaseContext context, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _dbContext = context;
            _appEnvironment = appEnvironment;
        }

        [HttpGet("file")]
        public async Task<VirtualFileResult> Get([Required] string name)
        {
            Modul modul = _dbContext.Moduls.FirstOrDefault(m => m.Name == name);
            if (modul != null)
            {
                return File("~" + modul.FilePath, "application/octet-stream", modul.Name + "." + modul.FileType);
            }
            return null;
        }

    }
}
