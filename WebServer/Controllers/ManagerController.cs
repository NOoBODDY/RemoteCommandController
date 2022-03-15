using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly ILogger<ManagerController> _logger;
        DataBaseContext _dbContext;
        IWebHostEnvironment _appEnvironment;

        public ManagerController(ILogger<ManagerController> logger, DataBaseContext context, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _dbContext = context;
            _appEnvironment = appEnvironment;
        }
        [HttpGet("commands")]
        public async Task<ActionResult<IEnumerable<Command>>> Get([Required]string guid)
        {
            RemoteComputer computer = _dbContext.RemoteComputers.FirstOrDefault(u => u.GUID == guid);
            
            
            if (computer != null)
            {
                DateTime lastconnection = computer.LastConnection;
                computer.LastConnection = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                List<Command> commands = _dbContext.Commands.Where(u => u.RemoteComputerId == computer.Id && u.TimeCreation > lastconnection).ToList();
                return commands;
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            Guid guid = Guid.NewGuid();
            RemoteComputer computer = new RemoteComputer();
            computer.GUID = guid.ToString();
            computer.LastConnection = DateTime.UtcNow;
            _dbContext.RemoteComputers.Add(computer);
            await _dbContext.SaveChangesAsync();
            List<User> Admins = await _dbContext.Users.Include(u => u.Role).Include(t=> t.UserParamsForRemotes).Where(r => r.Role.Name == "admin").ToListAsync();
            foreach (User u in Admins)
            {
                u.UserParamsForRemotes.Add(new UserParamsForRemote { ComputerName = "noname", RemoteComputer = computer });
            }
            _dbContext.Users.UpdateRange(Admins);
            _dbContext.SaveChanges();
            return guid.ToString();

        }
        [HttpGet("file")]
        public async Task<ActionResult<string>> Get([Required] int id)
        {
            Modul modul = await _dbContext.Moduls.FirstOrDefaultAsync(m => m.Id == id);
            if (modul != null)
            {
                string file_path = Path.Combine(_appEnvironment.ContentRootPath, modul.FilePath);
                return PhysicalFile(file_path, $"application/{modul.FileType}");
            }
            return NotFound();
        }


    }
}
