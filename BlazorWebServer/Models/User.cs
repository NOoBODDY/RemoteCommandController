using System;
using System.Collections.Generic;

namespace BlazorWebServer.Models
{
    public partial class User
    {
        public User()
        {
            Commands = new HashSet<Command>();
            Moduls = new HashSet<Modul>();
            UsersParamsForRemotes = new HashSet<UsersParamsForRemote>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Command> Commands { get; set; }
        public virtual ICollection<Modul> Moduls { get; set; }
        public virtual ICollection<UsersParamsForRemote> UsersParamsForRemotes { get; set; }
    }
}
