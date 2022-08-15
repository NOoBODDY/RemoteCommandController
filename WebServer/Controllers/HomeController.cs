using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebServer.Models;
using WebServer.Repositories;
using WebServer.Services;

namespace WebServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IUserParamsForRemoteService _userParamsForRemoteService;
        private readonly IRemoteComputerService _remoteComputerService;

        public HomeController(ILogger<HomeController> logger, IUserService userService,
            IUserParamsForRemoteService userParamsForRemoteService, IRemoteComputerService remoteComputerService)
        {
            _logger = logger;
            _userService = userService;
            _userParamsForRemoteService = userParamsForRemoteService;
            _remoteComputerService = remoteComputerService;
        }

        [Authorize(Roles="admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
            User curentUser = await _userService.GetUserByName(userName);
            var parameters = await _userParamsForRemoteService.GetParamsByUserId(curentUser.Id);
            List<ComputerViewModel> computers = new List<ComputerViewModel>();
            foreach (UserParamsForRemote? param in parameters)
            {
                ComputerViewModel computer = new ComputerViewModel();
                RemoteComputer remote = await _remoteComputerService.GetComputerById(param.RemoteComputerId);
                if (remote != null)
                {
                    computer.Id = param.RemoteComputerId;
                    computer.ComputerName = param.ComputerName;
                    computer.LastConnection = remote.LastConnection;
                    computers.Add(computer);
                }
            }
            return View(computers);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}