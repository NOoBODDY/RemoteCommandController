using ControlService.Core.Models;
using ControlService.Models;
using RestSharp;
using System.Text.Json;

namespace ControlService.Core
{
    public class Api
    {
        readonly string _guid;
        readonly string _baseUrl;// = "http://remote.offmysoap1.fvds.ru:7030/";
        readonly RestClient _client;

        public Api(SettingsService settings, IConfiguration configuration)
        {
            _baseUrl = configuration.GetConnectionString("BaseUrl");
            _client = new RestClient(_baseUrl);
            _guid = settings.Guid;
        }

        private Api(string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = new RestClient(_baseUrl);
        }

        public static Api GetApi(string baseUrl)
        {
            return new Api(baseUrl);
        }

        internal string GetGuid()
        {
            string url = "api/manager";
            RestRequest request = new RestRequest(url, Method.Get);
            var response = _client.GetAsync(request).Result;
            return response.Content.Trim('\"');
        }

        internal async Task<List<Command>> GetCommands()
        {
            string url = "api/manager/commands";
            RestRequest request = new RestRequest(url, Method.Get);
            request.AddParameter("guid", _guid);
            var resonse = await _client.GetAsync(request);
            List<Command> commands = JsonSerializer.Deserialize<Command[]>(resonse.Content).ToList();
            return commands;
        }

        internal void DownloadFile(string name)
        {
            string url = "api/download/file";
            RestRequest request = new RestRequest(url, Method.Get);
            request.AddParameter("name", name);
            if (!Directory.Exists("External"))
            {
                Directory.CreateDirectory("External");
            }
            File.WriteAllBytes($"External/{name}.dll", _client.DownloadDataAsync(request).Result);
        }

        internal string SendMessage(string text)
        {
            string url = "api/manager";
            RestRequest request = new RestRequest(url, Method.Post);
            Message message = new Message { Guid = _guid, Text = text, Time = DateTime.UtcNow };
            request.AddJsonBody<Message>(message);
            RestResponse response = _client.PostAsync(request).Result;
            return response.StatusCode.ToString();
        }
    }
}
