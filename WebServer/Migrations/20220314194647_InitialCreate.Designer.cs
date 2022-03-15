﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebServer.Models;

#nullable disable

namespace WebServer.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20220314194647_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ModulRemoteComputer", b =>
                {
                    b.Property<int>("ModulsId")
                        .HasColumnType("integer");

                    b.Property<int>("RemoteComputersId")
                        .HasColumnType("integer");

                    b.HasKey("ModulsId", "RemoteComputersId");

                    b.HasIndex("RemoteComputersId");

                    b.ToTable("ModulRemoteComputer");
                });

            modelBuilder.Entity("WebServer.Models.Command", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CommandText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RemoteComputerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeCreation")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RemoteComputerId");

                    b.HasIndex("UserId");

                    b.ToTable("Commands");
                });

            modelBuilder.Entity("WebServer.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RemoteComputerId")
                        .HasColumnType("integer");

                    b.Property<string>("message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RemoteComputerId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("WebServer.Models.Modul", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Moduls");
                });

            modelBuilder.Entity("WebServer.Models.RemoteComputer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("GUID")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LastConnection")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("RemoteComputers");
                });

            modelBuilder.Entity("WebServer.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "user"
                        });
                });

            modelBuilder.Entity("WebServer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "admin",
                            Password = "123456",
                            RoleId = 1
                        });
                });

            modelBuilder.Entity("WebServer.Models.UserParamsForRemote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ComputerName")
                        .HasColumnType("text");

                    b.Property<int>("RemoteComputerId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RemoteComputerId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersParamsForRemote");
                });

            modelBuilder.Entity("ModulRemoteComputer", b =>
                {
                    b.HasOne("WebServer.Models.Modul", null)
                        .WithMany()
                        .HasForeignKey("ModulsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebServer.Models.RemoteComputer", null)
                        .WithMany()
                        .HasForeignKey("RemoteComputersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebServer.Models.Command", b =>
                {
                    b.HasOne("WebServer.Models.RemoteComputer", "RemoteComputer")
                        .WithMany("Commands")
                        .HasForeignKey("RemoteComputerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebServer.Models.User", "User")
                        .WithMany("Commands")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RemoteComputer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebServer.Models.Message", b =>
                {
                    b.HasOne("WebServer.Models.RemoteComputer", "RemoteComputer")
                        .WithMany("Messages")
                        .HasForeignKey("RemoteComputerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RemoteComputer");
                });

            modelBuilder.Entity("WebServer.Models.Modul", b =>
                {
                    b.HasOne("WebServer.Models.User", "Author")
                        .WithMany("Moduls")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("WebServer.Models.User", b =>
                {
                    b.HasOne("WebServer.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("WebServer.Models.UserParamsForRemote", b =>
                {
                    b.HasOne("WebServer.Models.RemoteComputer", "RemoteComputer")
                        .WithMany("UserParamsForRemotes")
                        .HasForeignKey("RemoteComputerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebServer.Models.User", "User")
                        .WithMany("UserParamsForRemotes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RemoteComputer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebServer.Models.RemoteComputer", b =>
                {
                    b.Navigation("Commands");

                    b.Navigation("Messages");

                    b.Navigation("UserParamsForRemotes");
                });

            modelBuilder.Entity("WebServer.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("WebServer.Models.User", b =>
                {
                    b.Navigation("Commands");

                    b.Navigation("Moduls");

                    b.Navigation("UserParamsForRemotes");
                });
#pragma warning restore 612, 618
        }
    }
}