using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlService.Core.Models
{
    internal class Message
    {
        public string Text { get; set; }
        public string Guid { get; set; }
        public DateTime Time { get; set; }
    }
}
