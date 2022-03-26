using System;
using System.Collections.Generic;

namespace BlazorWebServer.Models
{
    public partial class Command
    {
        public int Id { get; set; }
        public string CommandText { get; set; } = null!;
        public DateTime TimeCreation { get; set; }
        public int UserId { get; set; }
        public int RemoteComputerId { get; set; }

        public virtual RemoteComputer RemoteComputer { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
