using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebServer.Models;
using WebServer.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebServer.Controllers
{
    public class ComputerController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        DataBaseContext _dbContext;

        public ComputerController(ILogger<HomeController> logger, DataBaseContext context)
        {
            _logger = logger;
            _dbContext = context;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Page(int id)
        {
            RemoteComputer computer = _dbContext.RemoteComputers.Include(c=> c.Moduls).ThenInclude(u=>u.Author).Include(m=> m.Messages).FirstOrDefault(u => u.Id == id);
            ComputerPageViewModel vm = new ComputerPageViewModel();
            vm.Id = computer.Id;
            vm.LastConnection = computer.LastConnection;

            string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
            User curentUser = _dbContext.Users.FirstOrDefault(u => u.Name == userName);
            UserParamsForRemote parameter = _dbContext.UsersParamsForRemote.FirstOrDefault(u => u.UserId == curentUser.Id && u.RemoteComputerId == id);
            vm.ComputerName = parameter.ComputerName;
            vm.UserId = curentUser.Id;
            vm.Moduls = computer.Moduls.ToList();
            vm.Messages = computer.Messages.ToList();
            return View(vm);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult SendCommand(ComputerPageViewModel vm, int id)
        {
            string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
            User curentUser = _dbContext.Users.FirstOrDefault(u => u.Name == userName);
            RemoteComputer computer = _dbContext.RemoteComputers.FirstOrDefault(u => u.Id == id);
            Command command = new Command();
            command.RemoteComputerId = computer.Id;
            command.UserId = curentUser.Id;
            command.TimeCreation = DateTime.UtcNow;
            command.CommandText = vm.Command;
            _dbContext.Commands.Add(command);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            RemoteComputer computer = _dbContext.RemoteComputers.FirstOrDefault(c => c.Id == id);
            if (computer != null)
            {
                _dbContext.RemoteComputers.Remove(computer);
                _dbContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ComputerViewModel computerVM = new ComputerViewModel();
            RemoteComputer computer = _dbContext.RemoteComputers.Include(comp => comp.UserParamsForRemotes).FirstOrDefault(c => c.Id == id);
            if (computer != null)
            {
                string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
                User curentUser = _dbContext.Users.FirstOrDefault(u => u.Name == userName);
                computerVM.Id = computer.Id;
                computerVM.ComputerName = computer.UserParamsForRemotes.FirstOrDefault(p => p.UserId == curentUser.Id).ComputerName;
                return View(computerVM);
            }
            return NotFound();
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Edit(ComputerViewModel computerVM)
        {
            if (ModelState.IsValid)
            {
                RemoteComputer computer = _dbContext.RemoteComputers.Include(comp => comp.UserParamsForRemotes).FirstOrDefault(c => c.Id == computerVM.Id);
                if (computer != null)
                {
                    string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
                    User curentUser = _dbContext.Users.FirstOrDefault(u => u.Name == userName);

                    UserParamsForRemote userParams = computer.UserParamsForRemotes.FirstOrDefault(p => p.UserId == curentUser.Id);
                    userParams.ComputerName = computerVM.ComputerName;
                    return RedirectToAction("Page", "Computer", computerVM.Id);
                }
            }
            return View(computerVM);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult AddModule(int id)
        {
            RemoteComputer computer = _dbContext.RemoteComputers.Include(x=> x.Moduls).FirstOrDefault(c => c.Id == id);
            AddModuleVM vm = new AddModuleVM { Modules = new SelectList (_dbContext.Moduls.ToList(), "Id","Name"), ComputerId = id };
            return View(vm);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult AddModule(AddModuleVM vm, int id)
        {
            if (ModelState.IsValid)
            {
                RemoteComputer computer = _dbContext.RemoteComputers.Include(x => x.Moduls).FirstOrDefault(c => c.Id == id);
                string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
                User curentUser = _dbContext.Users.FirstOrDefault(u => u.Name == userName);
                Modul modul = _dbContext.Moduls.FirstOrDefault(m => m.Id == vm.SelectedModuleId);
                computer.Moduls.Add(modul);
                _dbContext.RemoteComputers.Update(computer);
                Command command = new Command { RemoteComputerId = id, TimeCreation = DateTime.UtcNow, UserId = curentUser.Id, CommandText = $"install {modul.Name}" };
                _dbContext.Commands.Add(command);
                _dbContext.SaveChanges();
                return RedirectToAction("Page", "Computer", id);
            }
            return RedirectToAction("AddModule", id);
        }
    }
}
