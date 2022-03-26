using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorWebServer.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Command> Commands { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Modul> Moduls { get; set; } = null!;
        public virtual DbSet<RemoteComputer> RemoteComputers { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UsersParamsForRemote> UsersParamsForRemotes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Command>(entity =>
            {
                entity.HasIndex(e => e.RemoteComputerId, "IX_Commands_RemoteComputerId");

                entity.HasIndex(e => e.UserId, "IX_Commands_UserId");

                entity.HasOne(d => d.RemoteComputer)
                    .WithMany(p => p.Commands)
                    .HasForeignKey(d => d.RemoteComputerId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Commands)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasIndex(e => e.RemoteComputerId, "IX_Messages_RemoteComputerId");

                entity.Property(e => e.Message1).HasColumnName("message");

                entity.HasOne(d => d.RemoteComputer)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.RemoteComputerId);
            });

            modelBuilder.Entity<Modul>(entity =>
            {
                entity.HasIndex(e => e.AuthorId, "IX_Moduls_AuthorId");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Moduls)
                    .HasForeignKey(d => d.AuthorId);

                entity.HasMany(d => d.RemoteComputers)
                    .WithMany(p => p.Moduls)
                    .UsingEntity<Dictionary<string, object>>(
                        "ModulRemoteComputer",
                        l => l.HasOne<RemoteComputer>().WithMany().HasForeignKey("RemoteComputersId"),
                        r => r.HasOne<Modul>().WithMany().HasForeignKey("ModulsId"),
                        j =>
                        {
                            j.HasKey("ModulsId", "RemoteComputersId");

                            j.ToTable("ModulRemoteComputer");

                            j.HasIndex(new[] { "RemoteComputersId" }, "IX_ModulRemoteComputer_RemoteComputersId");
                        });
            });

            modelBuilder.Entity<RemoteComputer>(entity =>
            {
                entity.Property(e => e.Guid).HasColumnName("GUID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<UsersParamsForRemote>(entity =>
            {
                entity.ToTable("UsersParamsForRemote");

                entity.HasIndex(e => e.RemoteComputerId, "IX_UsersParamsForRemote_RemoteComputerId");

                entity.HasIndex(e => e.UserId, "IX_UsersParamsForRemote_UserId");

                entity.HasOne(d => d.RemoteComputer)
                    .WithMany(p => p.UsersParamsForRemotes)
                    .HasForeignKey(d => d.RemoteComputerId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UsersParamsForRemotes)
                    .HasForeignKey(d => d.UserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
