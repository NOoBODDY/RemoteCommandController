using Microsoft.AspNetCore.Mvc;
using WebServer.Models;
using WebServer.ViewModels;
using Microsoft.EntityFrameworkCore;

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
                moduleViewModel.FilePath = modul.FilePath;
                moduleViewModel.FileType = modul.FileType;
                return View(moduleViewModel);
            }
            return Content("Notfound");
        }
        [HttpPost]
        public IActionResult Create(ModuleViewModel moduleViewModel)
        {
            if(ModelState.IsValid)
            {
                if (moduleViewModel != null)
                {
                    string path = "/Files/" + moduleViewModel.FilePath;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        moduleViewModel.File.CopyTo(fileStream);
                    }
                    Modul modul = new Modul { FilePath = moduleViewModel.FilePath, Name = moduleViewModel.Name, FileType = moduleViewModel.FileType };
                    _dbContext.Moduls.Add(modul);
                    _dbContext.SaveChanges();
                    _dbContext.Attach(modul);
                    return RedirectToAction("Page", "Modul", modul.Id);
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
                moduleViewModel.FilePath = modul.FilePath;
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
                        string path = "/Files/" + moduleViewModel.FilePath + $"/{moduleViewModel.Name}.{moduleViewModel.FileType}";
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            moduleViewModel.File.CopyTo(fileStream);
                        }
                        modul.FilePath = moduleViewModel.FilePath;
                        modul.FileType = moduleViewModel.FileType;
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
            FileInfo info = new FileInfo(_appEnvironment.ContentRootPath + modul.FilePath);
            info.Delete();
            _dbContext.Moduls.Remove(modul);
            return RedirectToAction("Index", "Home");
        }
    }
}
