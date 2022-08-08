using ControlService.Core.Models;
using System.Text.Json;

namespace ControlService.Core
{
    public class SettingsService
    {

        private readonly IConfiguration _configuration;

        private string _fileName;

        private SettingsModel _settingModel;

        public string Guid
        {
            get
            {
                return _settingModel.Guid;
            }
            set
            {
                _settingModel.Guid = value;
                WriteToFile();
            }
        }
        public List<string> ExternalModules
        {
            get
            {
                return _settingModel.ExternalModules;
            }
            set
            {
                _settingModel.ExternalModules = value;
                WriteToFile();
            }
        }

        public int Delay
        {
            get
            {
                return _settingModel.Delay;
            }
            set
            {
                _settingModel.Delay = value;
                WriteToFile();
            }
        }

        public SettingsService(IConfiguration configuration)
        {

            _configuration = configuration;
            _fileName = _configuration.GetSection("FileName").Value;
            _settingModel = new SettingsModel();
            try
            {
                ReadFromFile();
            }
            catch (FileNotFoundException ex)
            {
                _settingModel.ExternalModules = new List<string>();
                _settingModel.Guid = Api.GetApi(_configuration.GetConnectionString("BaseUrl")).GetGuid();
                _settingModel.Delay = 30000;
                WriteToFile();
            }
        }


        private void WriteToFile()
        {
            using (StreamWriter writer = new StreamWriter(_fileName, false))
            {
                string text = JsonSerializer.Serialize(this);
                writer.WriteLine(text);
            }
        }

        private void ReadFromFile()
        {
            FileInfo info = new FileInfo(_fileName);
            if (info.Exists)
            {
                using (StreamReader reader = new StreamReader(_fileName))
                {
                    _settingModel = JsonSerializer.Deserialize<SettingsModel>(reader.ReadToEnd());
                }
            }
            else
            {
                throw new FileNotFoundException();
            }
        }


    }

}
