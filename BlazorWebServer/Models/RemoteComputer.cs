using System;
using System.Collections.Generic;

namespace BlazorWebServer.Models
{
    public partial class RemoteComputer
    {
        public RemoteComputer()
        {
            Commands = new HashSet<Command>();
            Messages = new HashSet<Message>();
            UsersParamsForRemotes = new HashSet<UsersParamsForRemote>();
            Moduls = new HashSet<Modul>();
        }

        public int Id { get; set; }
        public string Guid { get; set; } = null!;
        public DateTime LastConnection { get; set; }

        public virtual ICollection<Command> Commands { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<UsersParamsForRemote> UsersParamsForRemotes { get; set; }

        public virtual ICollection<Modul> Moduls { get; set; }
    }
}
