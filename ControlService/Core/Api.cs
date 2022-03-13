using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RestSharp;
using ControlService.Models;


namespace ControlService.Core
{
    internal class Api
    {
        public string Guid { get; set; }
        static string _baseUrl = "https://localhost:5001";
        RestClient _client;

        internal Api(string guid)
        {
            _client = new RestClient(_baseUrl);
            
            if (guid != null && guid != "")
            {
                Guid = guid;
            }
            else
            {
                GetGuid();
            }
            
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



    }
}
