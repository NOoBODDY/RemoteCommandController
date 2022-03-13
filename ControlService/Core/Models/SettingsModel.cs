using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlService.ExternalModules;

namespace ControlService.Core.Models
{
    internal class SettingsModel
    {
        public string Guid { get; set; }
        public Dictionary<string, IExternalModule> ExternalModules { get; set; }
    }
}
