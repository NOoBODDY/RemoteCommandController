using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using WebServer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebServer.Repositories;
using WebServer.Services;

namespace WebServer.Controllers
{
    public class ModulController : Controller
    {
        private readonly ILogger<ModulController> _logger;
        private readonly IModuleService _moduleService;
        private readonly IUserService _userService;
        IWebHostEnvironment _appEnvironment;

        public ModulController(ILogger<ModulController> logger, IWebHostEnvironment appEnvironment, IModuleService moduleService, IUserService userService)
        {
            _logger = logger;
            _appEnvironment = appEnvironment;
            _moduleService = moduleService;
            _userService = userService;
        }
        
        public async Task<IActionResult> Index()
        {
            

            return View((await _moduleService.GetAllModulesWithAuthor()).ToList());
        }

        [HttpGet]
        public async Task<IActionResult> Page(int id)
        {
            Module module = await _moduleService.GetModuleById(id);
            if (module != null)
            {
                ModuleViewModel moduleViewModel = new ModuleViewModel
                {
                    Id = module.Id,
                    Name = module.Name,
                    FileType = module.FileType
                };
                return View(moduleViewModel);
            }
            return Content("Notfound");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(ModuleViewModel moduleViewModel)
        {
            if(ModelState.IsValid)
            {
                if (moduleViewModel != null)
                {
                    string path = "/Files/" + moduleViewModel.File.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        moduleViewModel.File.CopyTo(fileStream);
                    }
                    string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
                    User curentUser = await _userService.GetUserByName(userName);
                    Module module = new Module { FilePath = path, Name = moduleViewModel.Name, FileType = moduleViewModel.FileType, Author = curentUser };
                    await _moduleService.AddNewModule(module);
                    return RedirectToAction("Index", "Modul", module.Id);
                }
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Module module = await _moduleService.GetModuleById(id);
            if (module != null)
            {
                ModuleViewModel moduleViewModel = new ModuleViewModel
                {
                    Id = module.Id,
                    Name = module.Name,
                    FileType = module.FileType
                };
                return View(moduleViewModel);
            }
            return Content("Notfound");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ModuleViewModel moduleViewModel)
        {
            if(ModelState.IsValid)
            {
                if (moduleViewModel != null)
                {
                    Module module = await _moduleService.GetModuleById(moduleViewModel.Id);
                    module.Name = moduleViewModel.Name;
                    if (moduleViewModel.File != null)
                    {
                        string path = "/Files/" + moduleViewModel.File.Name;
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            moduleViewModel.File.CopyTo(fileStream);
                        }
                        module.FileType = moduleViewModel.FileType;
                        string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
                        User curentUser = await _userService.GetUserByName(userName);
                        module.Author = curentUser;
                    }

                    await _moduleService.UpdateModule(module);
                    return RedirectToAction("Page", "Modul", module.Id);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Module module = await _moduleService.GetModuleById(id);
            if (module == null) return RedirectToAction("Index", "Home");
            _moduleService.DeleteModule(module);
            FileInfo info = new FileInfo(_appEnvironment.WebRootPath + module.FilePath);
            info.Delete();
            
            return RedirectToAction("Index", "Home");
        }
    }
}
