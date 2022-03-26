using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using WebServer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebServer.Controllers
{
    public class ModulController : Controller
    {
        private readonly ILogger<ModulController> _logger;
        DataBaseContext _dbContext;
        IWebHostEnvironment _appEnvironment;

        public ModulController(ILogger<ModulController> logger, DataBaseContext context, IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            _dbContext = context;
            _appEnvironment = appEnvironment;
        }
        
        public IActionResult Index()
        {
            

            return View(_dbContext.Moduls.Include(m=> m.Author).ToList());
        }

        [HttpGet]
        public IActionResult Page(int id)
        {
            Modul modul = _dbContext.Moduls.FirstOrDefault(m => m.Id == id);
            if (modul != null)
            {
                ModuleViewModel moduleViewModel = new ModuleViewModel();
                moduleViewModel.Id = modul.Id;
                moduleViewModel.Name = modul.Name;
                moduleViewModel.FileType = modul.FileType;
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
        public IActionResult Create(ModuleViewModel moduleViewModel)
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
                    User curentUser = _dbContext.Users.FirstOrDefault(u => u.Name == userName);
                    Modul modul = new Modul { FilePath = path, Name = moduleViewModel.Name, FileType = moduleViewModel.FileType, Author = curentUser };
                    _dbContext.Moduls.Add(modul);
                    _dbContext.SaveChanges();
                    _dbContext.Attach(modul);
                    return RedirectToAction("Index", "Modul", modul.Id);
                }
            }
            
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Modul modul = _dbContext.Moduls.FirstOrDefault(m => m.Id == id);
            if (modul != null)
            {
                ModuleViewModel moduleViewModel = new ModuleViewModel();
                moduleViewModel.Id = modul.Id;
                moduleViewModel.Name = modul.Name;
                moduleViewModel.FileType = modul.FileType;
                return View(moduleViewModel);
            }
            return Content("Notfound");
        }

        [HttpPost]
        public IActionResult Edit(ModuleViewModel moduleViewModel)
        {
            if(ModelState.IsValid)
            {
                if (moduleViewModel != null)
                {
                    Modul modul = _dbContext.Moduls.FirstOrDefault(m => m.Id == moduleViewModel.Id);
                    modul.Name = moduleViewModel.Name;
                    if (moduleViewModel.File != null)
                    {
                        string path = "/Files/" + moduleViewModel.File.Name;
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            moduleViewModel.File.CopyTo(fileStream);
                        }
                        modul.FileType = moduleViewModel.FileType;
                        string userName = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Subject.Name;
                        User curentUser = _dbContext.Users.FirstOrDefault(u => u.Name == userName);
                        modul.Author = curentUser;
                    }

                    _dbContext.Moduls.Update(modul);
                    _dbContext.SaveChanges();
                    return RedirectToAction("Page", "Modul", modul.Id);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Modul modul = _dbContext.Moduls.FirstOrDefault(m => m.Id == id);
            if (modul == null) return RedirectToAction("Index", "Home");
            _dbContext.Moduls.Remove(modul);
            _dbContext.SaveChanges();
            FileInfo info = new FileInfo(_appEnvironment.WebRootPath + modul.FilePath);
            info.Delete();
            
            return RedirectToAction("Index", "Home");
        }
    }
}
