using ExternalModule;
using System.Reflection;

namespace ControlService.Core
{
    public class ModuleFabric
    {

        //TODO: What about core?
        private readonly SettingsService _settingsService;
        private Dictionary<string, IExternalModule> _modules;

        public ModuleFabric(SettingsService settingsService /*, Core core*/)
        {
            _modules = new Dictionary<string, IExternalModule>();
            //_modules.Add("core", core);
            _settingsService = settingsService;
            ImportModules(_settingsService.ExternalModules);
        }
        private void ImportModules(List<string> externalModules)
        {
            _modules = new Dictionary<string, IExternalModule>();
            foreach (var moduleName in externalModules)
            {
                _modules.Add(moduleName, LoadModule(moduleName));
            }

        }

        public IExternalModule CreateInstance(string commandWord)
        {
            if (_modules.ContainsKey(commandWord))
            {
                return _modules[commandWord];
            }
            try
            {
                IExternalModule module = LoadModule(commandWord);
                _modules.Add(commandWord, module);
                _settingsService.ExternalModules.Add(commandWord);
                return module;
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception($"{commandWord}: no such module");
            }
            catch (FileLoadException ex)
            {
                throw new Exception($"{commandWord}: bad module file");
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{commandWord}: bad argument", ex);
            }

        }

        private IExternalModule LoadModule(string moduleName)
        {
            Assembly asm = Assembly.LoadFrom("External/" + moduleName + ".dll");
            Type? t = asm.GetType($"{moduleName}.{moduleName}");
            object? obj = Activator.CreateInstance(t);
            return (IExternalModule)obj;
        }
    }

}
