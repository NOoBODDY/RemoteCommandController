using System;
using System.Collections.Generic;

namespace BlazorWebServer.Models
{
    public partial class UsersParamsForRemote
    {
        public int Id { get; set; }
        public string? ComputerName { get; set; }
        public int UserId { get; set; }
        public int RemoteComputerId { get; set; }

        public virtual RemoteComputer RemoteComputer { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
