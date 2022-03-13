using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebServer.Models;

namespace WebServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        DataBaseContext _dbContext;

        public HomeController(ILogger<HomeController> logger, DataBaseContext context)
        {
            _logger = logger;
            _dbContext = context;
        }
        [Authorize(Roles="admin")]
        public IActionResult Index()
        {
            string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
            User curentUser = _dbContext.Users.FirstOrDefault(u => u.Name == userName);
            List<UserParamsForRemote> parameters = _dbContext.UsersParamsForRemote.Where(u => u.UserId == curentUser.Id).ToList();
            List<ComputerViewModel> computers = new List<ComputerViewModel>();
            foreach (UserParamsForRemote param in parameters)
            {
                ComputerViewModel computer = new ComputerViewModel();
                RemoteComputer remote = _dbContext.RemoteComputers.FirstOrDefault(u => u.Id == param.RemoteComputerId);
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