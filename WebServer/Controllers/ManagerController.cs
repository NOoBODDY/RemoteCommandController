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
    public class ManagerController : ControllerBase
    {
        private readonly ILogger<ManagerController> _logger;
        private readonly IUserService _userService;
        private readonly IRemoteComputerService _remoteComputerService;
        private readonly ICommandService _commandService;
        private readonly IMessageService _messageService;
        IWebHostEnvironment _appEnvironment;

        public ManagerController(ILogger<ManagerController> logger, IWebHostEnvironment appEnvironment,
            IUserService userService, IRemoteComputerService remoteComputerService, ICommandService commandService,
            IMessageService messageService)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            _userService = userService;
            _remoteComputerService = remoteComputerService;
            _commandService = commandService;
            _messageService = messageService;
        }

        [HttpGet("commands")]
        public async Task<ActionResult<IEnumerable<Command>>> Get([Required]string guid)
        {
            RemoteComputer computer = await _remoteComputerService.GetComputerByGUID(guid);
            
            if (computer != null)
            {
                DateTime lastconnection = computer.LastConnection;
                await _remoteComputerService.UpdateConnectionTimeById(computer.Id);
                return await  _commandService.GetLastCommandForComputer(computer.Id ,lastconnection);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var computer = await _remoteComputerService.AddNewComputer();
            var Admins = await _userService.GetAdminsWithUserParams();
            foreach (User u in Admins)
            {
                u.UserParamsForRemotes.Add(new UserParamsForRemote { ComputerName = "noname", RemoteComputer = computer });
            }

            await _userService.UpdateUser(Admins);
            return computer.GUID;

        }
        
        [HttpPost]
        public async Task <IActionResult> Post(MessageFromComputer message)
        {
            RemoteComputer computer = await _remoteComputerService.GetComputerByGUID(message.Guid);
            if (computer != null)
            {
                computer.LastConnection = DateTime.UtcNow;
                Message mes = new Message { RemoteComputerId = computer.Id, message = message.Text, DateTime = message.Time };
                await _messageService.AddNewMessage(mes);
                return Ok();
            }
            return NotFound();
        }




    }
}
