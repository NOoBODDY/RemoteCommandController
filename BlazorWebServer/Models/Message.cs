using System;
using System.Collections.Generic;

namespace BlazorWebServer.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public string Message1 { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public int RemoteComputerId { get; set; }

        public virtual RemoteComputer RemoteComputer { get; set; } = null!;
    }
}
