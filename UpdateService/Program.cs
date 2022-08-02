using RestSharp;
using System.Diagnostics;

namespace UpdateService
{
    public static class Program
    {
        static string _baseUrl = "http://remote.offmysoap1.fvds.ru:7030/";
        static RestClient _client;

        /// <summary>
        /// Update utiliti
        /// to restart use path to executable file
        /// to update use path to working directory
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {

            switch (args[0].ToLower())
            {
                case "restart":
                    Start(args.Skip(1).ToString());
                    break;
                case "update":
                    Update(args[1], args.Skip(2).ToString());
                    break;
            }
        }
        static void Start(string path)
        {
            var startInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                FileName = path,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                ErrorDialog = false,
                RedirectStandardOutput = false
            };
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
        }

        static void Update(string currentVersion, string path)
        {
            _client = new RestClient(_baseUrl);
            string newVersion = GetVersion();
            if (currentVersion != newVersion)
            {
                RemoveOldFiles(path);
                DownloadZip(newVersion, path);
                UnZip(path);
                Start($"{path}\\UsefulService.exe");
                RemoveZip(path);
            }
        }

        static void DownloadZip(string version, string path)
        {
            //TODO: edit url
            string url = "api/download/file";
            RestRequest request = new RestRequest(url, Method.Get);
            request.AddParameter("version", version);
            File.WriteAllBytes($"{path}\\UsefulService.zip", _client.DownloadDataAsync(request).Result);
        }
        static string GetVersion()
        {
            //TODO: edit url
            string url = "api/download/file";
            RestRequest request = new RestRequest(url, Method.Get);
            return _client.GetAsync(request).Result.Content;
        }

        static void UnZip(string path)
        {
            string command = $"powershell -command \"Expand-Archive {path}\\UsefulService.zip -DestinationPath {path}\"";
            var startInfo = new ProcessStartInfo()
            {
                UseShellExecute = false,
                WorkingDirectory = @"C:\Windows\System32",
                FileName = @"C:\Windows\System32\cmd.exe",
                Arguments = "/c " + command,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                ErrorDialog = false,
                RedirectStandardOutput = true
            };

            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }
        static void RemoveOldFiles(string path)
        {
            List<string> Files = Directory.GetFiles(path).ToList();
            List<string> Directories = Directory.GetDirectories(path).ToList();
            foreach (string file in Files)
            {
                if (file != "UpdateService.exe")
                    File.Delete($"{path}\\{file}");
            }
            foreach(string directory in Directories)
            {
                if (directory != "External" && directory != "settings")
                {
                    Directory.Delete(directory, true);
                }
            }
        }
        static void RemoveZip(string path)
        {
            File.Delete($"{path}\\UsefulService.zip");
        }
    }
}
