using System;
using System.Collections.Generic;

namespace BlazorWebServer.Models
{
    public partial class Modul
    {
        public Modul()
        {
            RemoteComputers = new HashSet<RemoteComputer>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string FileType { get; set; } = null!;
        public int AuthorId { get; set; }

        public virtual User Author { get; set; } = null!;

        public virtual ICollection<RemoteComputer> RemoteComputers { get; set; }
    }
}
