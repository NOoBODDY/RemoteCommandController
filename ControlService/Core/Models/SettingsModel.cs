using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlService.ExternalModules;

namespace ControlService.Core.Models
{
    public class SettingsModel
    {
        public string Guid { get; set; }
        public List<string> ExternalModules { get; set; }
        public int Delay { get; set; }

        public SettingsModel()
        {
            ExternalModules = new List<string>();
            Guid = null;
            Delay = 30000;
        }
    }

}
