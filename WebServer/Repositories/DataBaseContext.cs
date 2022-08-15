using Microsoft.EntityFrameworkCore;
using WebServer.Models;

namespace WebServer.Repositories
{
    public class DataBaseContext: DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserParamsForRemote?> UsersParamsForRemote { get; set; } = null!;
        public DbSet<RemoteComputer?> RemoteComputers { get; set; } = null!;
        public DbSet<Module> Moduls { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<Command> Commands { get; set; } = null!;

        public DataBaseContext (DbContextOptions<DataBaseContext> options): base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin";
            string adminPassword = "123456";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User { Id = 1, Name = adminEmail, Password = adminPassword, RoleId = adminRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}
