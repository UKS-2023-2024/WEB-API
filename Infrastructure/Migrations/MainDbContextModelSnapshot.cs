﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Auth.Email", b =>
                {
                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Value");

                    b.HasIndex("UserId");

                    b.ToTable("Emails");
                });

            modelBuilder.Entity("Domain.Auth.SocialAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SocialAccounts");
                });

            modelBuilder.Entity("Domain.Auth.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Bio")
                        .HasColumnType("text");

                    b.Property<string>("Company")
                        .HasColumnType("text");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PrimaryEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Website")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryEmail")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Organizations.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ContactEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("RoleName");

                    b.ToTable("OrganizationMembers");
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationPermission", b =>
                {
                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Value");

                    b.ToTable("OrganizationPermissions");

                    b.HasData(
                        new
                        {
                            Value = "owner",
                            Description = ""
                        },
                        new
                        {
                            Value = "admin",
                            Description = ""
                        },
                        new
                        {
                            Value = "manager",
                            Description = ""
                        },
                        new
                        {
                            Value = "read_only",
                            Description = ""
                        });
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationRole", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("OrganizationRoles");

                    b.HasData(
                        new
                        {
                            Name = "OWNER",
                            Description = "Has all rights!"
                        },
                        new
                        {
                            Name = "MEMBER",
                            Description = "Member has all rights except owners"
                        });
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationRolePermission", b =>
                {
                    b.Property<string>("RoleName")
                        .HasColumnType("text");

                    b.Property<string>("PermissionName")
                        .HasColumnType("text");

                    b.HasKey("RoleName", "PermissionName");

                    b.HasIndex("PermissionName");

                    b.ToTable("OrganizationRolePermissions");

                    b.HasData(
                        new
                        {
                            RoleName = "OWNER",
                            PermissionName = "owner"
                        },
                        new
                        {
                            RoleName = "OWNER",
                            PermissionName = "admin"
                        },
                        new
                        {
                            RoleName = "OWNER",
                            PermissionName = "manager"
                        },
                        new
                        {
                            RoleName = "OWNER",
                            PermissionName = "read_only"
                        },
                        new
                        {
                            RoleName = "MEMBER",
                            PermissionName = "manager"
                        },
                        new
                        {
                            RoleName = "MEMBER",
                            PermissionName = "read_only"
                        });
                });

            modelBuilder.Entity("Domain.Repositories.Repository", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("OrganizationId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Repositories");
                });

            modelBuilder.Entity("Domain.Repositories.RepositoryMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RepositoryId")
                        .HasColumnType("uuid");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("RepositoryId");

                    b.ToTable("RepositoryMembers");
                });

            modelBuilder.Entity("OrganizationUser", b =>
                {
                    b.Property<Guid>("PendingMembersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PendingOrganizationsId")
                        .HasColumnType("uuid");

                    b.HasKey("PendingMembersId", "PendingOrganizationsId");

                    b.HasIndex("PendingOrganizationsId");

                    b.ToTable("OrganizationUser");
                });

            modelBuilder.Entity("RepositoryUser", b =>
                {
                    b.Property<Guid>("PendingMembersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PendingRepositoriesId")
                        .HasColumnType("uuid");

                    b.HasKey("PendingMembersId", "PendingRepositoriesId");

                    b.HasIndex("PendingRepositoriesId");

                    b.ToTable("RepositoryUser");
                });

            modelBuilder.Entity("RepositoryUser1", b =>
                {
                    b.Property<Guid>("StarredById")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StarredId")
                        .HasColumnType("uuid");

                    b.HasKey("StarredById", "StarredId");

                    b.HasIndex("StarredId");

                    b.ToTable("RepositoryUser1");
                });

            modelBuilder.Entity("Domain.Auth.Email", b =>
                {
                    b.HasOne("Domain.Auth.User", "User")
                        .WithMany("SecondaryEmails")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Auth.SocialAccount", b =>
                {
                    b.HasOne("Domain.Auth.User", null)
                        .WithMany("SocialAccounts")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationMember", b =>
                {
                    b.HasOne("Domain.Auth.User", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Organizations.Organization", "Organization")
                        .WithMany("Members")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Organizations.OrganizationRole", "Role")
                        .WithMany("Members")
                        .HasForeignKey("RoleName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("Organization");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationRolePermission", b =>
                {
                    b.HasOne("Domain.Organizations.OrganizationPermission", "Permission")
                        .WithMany("Roles")
                        .HasForeignKey("PermissionName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Organizations.OrganizationRole", "Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Domain.Repositories.Repository", b =>
                {
                    b.HasOne("Domain.Organizations.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Domain.Repositories.RepositoryMember", b =>
                {
                    b.HasOne("Domain.Auth.User", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Repositories.Repository", "Repository")
                        .WithMany("Members")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("Repository");
                });

            modelBuilder.Entity("OrganizationUser", b =>
                {
                    b.HasOne("Domain.Auth.User", null)
                        .WithMany()
                        .HasForeignKey("PendingMembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Organizations.Organization", null)
                        .WithMany()
                        .HasForeignKey("PendingOrganizationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RepositoryUser", b =>
                {
                    b.HasOne("Domain.Auth.User", null)
                        .WithMany()
                        .HasForeignKey("PendingMembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Repositories.Repository", null)
                        .WithMany()
                        .HasForeignKey("PendingRepositoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RepositoryUser1", b =>
                {
                    b.HasOne("Domain.Auth.User", null)
                        .WithMany()
                        .HasForeignKey("StarredById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Repositories.Repository", null)
                        .WithMany()
                        .HasForeignKey("StarredId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Auth.User", b =>
                {
                    b.Navigation("SecondaryEmails");

                    b.Navigation("SocialAccounts");
                });

            modelBuilder.Entity("Domain.Organizations.Organization", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationPermission", b =>
                {
                    b.Navigation("Roles");
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationRole", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("Permissions");
                });

            modelBuilder.Entity("Domain.Repositories.Repository", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
