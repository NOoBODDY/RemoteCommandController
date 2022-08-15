using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebServer.Models;
using WebServer.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebServer.Repositories;
using WebServer.Services;

namespace WebServer.Controllers
{
    public class ComputerController : Controller
    {
        private readonly ILogger<ComputerController> _logger;
        private readonly IRemoteComputerService _remoteComputerService;
        private readonly IUserService _userService;
        private readonly IUserParamsForRemoteService _userParamsForRemoteService;
        private readonly ICommandService _commandService;
        private readonly IModuleService _moduleService;
        public ComputerController(ILogger<ComputerController> logger,
            IRemoteComputerService remoteComputerService, IUserService userService,
            IUserParamsForRemoteService userParamsForRemoteService, ICommandService commandService,
            IModuleService moduleService)
        {
            _logger = logger;
            _remoteComputerService = remoteComputerService;
            _userService = userService;
            _userParamsForRemoteService = userParamsForRemoteService;
            _commandService = commandService;
            _moduleService = moduleService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Page(int id)
        {
            RemoteComputer computer = await _remoteComputerService.GetComputerByIdWithModulsWithAuthorWithMessages(id);
            

            string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
            User curentUser = await _userService.GetUserByName(userName);
            UserParamsForRemote parameter =
                await _userParamsForRemoteService.GetParamsByUserIdAndComputerId(curentUser.Id, computer.Id);
            ComputerPageViewModel vm = new ComputerPageViewModel
            {
                Id = computer.Id,
                LastConnection = computer.LastConnection,
                ComputerName = parameter.ComputerName,
                UserId = curentUser.Id,
                Moduls = computer.Modules.ToList(),
                Messages = computer.Messages.ToList()
            };
            return View(vm);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> SendCommand(ComputerPageViewModel vm, int id)
        {
            string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
            User curentUser = await _userService.GetUserByName(userName);
            RemoteComputer computer = await _remoteComputerService.GetComputerById(id);
            Command command = new Command
            {
                RemoteComputerId = computer.Id,
                UserId = curentUser.Id,
                TimeCreation = DateTime.UtcNow,
                CommandText = vm.Command
            };
            _commandService.AddCommand(command);
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _remoteComputerService.DeleteComputerById(id);
                return RedirectToAction("Index", "Home");
            }
            catch (ArgumentNullException e)
            {
                return NotFound();
            }
            
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ComputerViewModel computerVM = new ComputerViewModel();

            try
            {
                string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
                User curentUser = await _userService.GetUserByName(userName);
                computerVM.Id = id;
                computerVM.ComputerName = await _remoteComputerService.GetComputerNameForUserById(id, curentUser.Id);
                return View(computerVM);
            }
            catch (ArgumentNullException e)
            {
                return NotFound();
            }
                
                
                
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(ComputerViewModel computerVM)
        {
            if (ModelState.IsValid)
            {
                RemoteComputer? computer = await _remoteComputerService.GetComputerByIdWithParams(computerVM.Id);
                if (computer != null)
                {
                    string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
                    User curentUser = await _userService.GetUserByName(userName);

                    UserParamsForRemote? userParams = computer.UserParamsForRemotes.FirstOrDefault(p => p.UserId == curentUser.Id);
                    userParams.ComputerName = computerVM.ComputerName;
                    _userParamsForRemoteService.SaveParams(userParams);
                    return RedirectToAction("Page", "Computer", new { id = computerVM.Id });
                }
            }
            return View(computerVM);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> AddModule(int id)
        {
            AddModuleVM vm = new AddModuleVM { Modules = new SelectList (await _moduleService.GetAllModules(), "Id","Name"), ComputerId = id };
            return View(vm);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddModule(AddModuleVM vm, int id)
        {
            if (ModelState.IsValid)
            {
                string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
                User curentUser = await _userService.GetUserByName(userName);
                Module module = await _moduleService.GetModuleById(vm.SelectedModuleId);
                _remoteComputerService.AddModule(module, id);
                Command command = new Command
                {
                    RemoteComputerId = id, TimeCreation = DateTime.UtcNow, UserId = curentUser.Id,
                    CommandText = $"core install {module.Name}"
                };
                _commandService.AddCommand(command);
                return RedirectToAction("Page", "Computer", new { id = id });
            }
            return RedirectToAction("AddModule", new { id = id });
        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> CreateScript(int id)
        {
            ScriptVM vm = new ScriptVM { Id = id };
            return View(vm);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateScript(ScriptVM vm, int id)
        {
            if (ModelState.IsValid)
            {
                string commandToCreateScript = $"ConsoleModule startmodule echo {vm.FileText} >> {vm.FilePath}/{vm.FileName}";
                string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
                User curentUser = await _userService.GetUserByName(userName);
                Command command = new Command { CommandText = commandToCreateScript, RemoteComputerId = id, TimeCreation = DateTime.UtcNow, UserId = curentUser.Id};
                _commandService.AddCommand(command);
                return RedirectToAction("Page", "Computer", new { id = id });
            }
            
            return RedirectToAction("CreateScript", new { id = id });
        }
    }
}
