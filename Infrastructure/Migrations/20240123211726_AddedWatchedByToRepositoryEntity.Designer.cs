﻿// <auto-generated />
using System;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20240123211726_AddedWatchedByToRepositoryEntity")]
    partial class AddedWatchedByToRepositoryEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Domain.Branches.Branch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RepositoryId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("RepositoryId");

                    b.HasIndex("Name", "RepositoryId")
                        .IsUnique();

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("Domain.Milestones.Milestone", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Closed")
                        .HasColumnType("boolean");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly?>("DueDate")
                        .HasColumnType("date");

                    b.Property<Guid>("RepositoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RepositoryId");

                    b.ToTable("Milestones");
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

            modelBuilder.Entity("Domain.Organizations.OrganizationInvite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("OrganizationId", "UserId")
                        .IsUnique();

                    b.ToTable("OrganizationInvites");
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationMember", b =>
                {
                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("OrganizationId", "MemberId");

                    b.HasIndex("MemberId");

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

            modelBuilder.Entity("Domain.Repositories.RepositoryInvite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("RepositoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("RepositoryId", "UserId")
                        .IsUnique();

                    b.ToTable("RepositoryInvites");
                });

            modelBuilder.Entity("Domain.Repositories.RepositoryMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean");

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

            modelBuilder.Entity("Domain.Tasks.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<int>("EventType")
                        .HasColumnType("integer");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("TaskId");

                    b.ToTable("Events");

                    b.HasDiscriminator<int>("EventType").HasValue(0);

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Domain.Tasks.Label", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RepositoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RepositoryId");

                    b.HasIndex("Title", "RepositoryId")
                        .IsUnique();

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("Domain.Tasks.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("MilestoneId")
                        .HasColumnType("uuid");

                    b.Property<int>("Number")
                        .HasColumnType("integer");

                    b.Property<Guid>("RepositoryId")
                        .HasColumnType("uuid");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("MilestoneId");

                    b.HasIndex("RepositoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Tasks");

                    b.HasDiscriminator<int>("Type");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("LabelTask", b =>
                {
                    b.Property<Guid>("LabelsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TasksId")
                        .HasColumnType("uuid");

                    b.HasKey("LabelsId", "TasksId");

                    b.HasIndex("TasksId");

                    b.ToTable("LabelTask");
                });

            modelBuilder.Entity("RepositoryMemberTask", b =>
                {
                    b.Property<Guid>("AssigneesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TasksId")
                        .HasColumnType("uuid");

                    b.HasKey("AssigneesId", "TasksId");

                    b.HasIndex("TasksId");

                    b.ToTable("RepositoryMemberTask");
                });

            modelBuilder.Entity("RepositoryUser", b =>
                {
                    b.Property<Guid>("StarredById")
                        .HasColumnType("uuid");

                    b.Property<Guid>("StarredId")
                        .HasColumnType("uuid");

                    b.HasKey("StarredById", "StarredId");

                    b.HasIndex("StarredId");

                    b.ToTable("RepositoryUser");
                });

            modelBuilder.Entity("RepositoryUser1", b =>
                {
                    b.Property<Guid>("WatchedById")
                        .HasColumnType("uuid");

                    b.Property<Guid>("WatchedId")
                        .HasColumnType("uuid");

                    b.HasKey("WatchedById", "WatchedId");

                    b.HasIndex("WatchedId");

                    b.ToTable("RepositoryUser1");
                });

            modelBuilder.Entity("Domain.Tasks.AssignEvent", b =>
                {
                    b.HasBaseType("Domain.Tasks.Event");

                    b.Property<Guid>("AssigneeId")
                        .HasColumnType("uuid");

                    b.HasIndex("AssigneeId");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Domain.Tasks.UnassignEvent", b =>
                {
                    b.HasBaseType("Domain.Tasks.Event");

                    b.Property<Guid>("AssigneeId")
                        .HasColumnType("uuid");

                    b.HasIndex("AssigneeId");

                    b.ToTable("Events", t =>
                        {
                            t.Property("AssigneeId")
                                .HasColumnName("UnassignEvent_AssigneeId");
                        });

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("Domain.Tasks.Issue", b =>
                {
                    b.HasBaseType("Domain.Tasks.Task");

                    b.HasDiscriminator().HasValue(0);
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

            modelBuilder.Entity("Domain.Branches.Branch", b =>
                {
                    b.HasOne("Domain.Auth.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Repositories.Repository", "Repository")
                        .WithMany("Branches")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Repository");
                });

            modelBuilder.Entity("Domain.Milestones.Milestone", b =>
                {
                    b.HasOne("Domain.Repositories.Repository", "Repository")
                        .WithMany("Milestones")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Repository");
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationInvite", b =>
                {
                    b.HasOne("Domain.Organizations.Organization", "Organization")
                        .WithMany("PendingInvites")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Auth.User", "User")
                        .WithMany("OrganizationInvites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationMember", b =>
                {
                    b.HasOne("Domain.Auth.User", "Member")
                        .WithMany("Members")
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
                        .WithMany("Repositories")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Domain.Repositories.RepositoryInvite", b =>
                {
                    b.HasOne("Domain.Repositories.Repository", "Repository")
                        .WithMany("PendingInvites")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Auth.User", "User")
                        .WithMany("RepositoryInvites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Repository");

                    b.Navigation("User");
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

            modelBuilder.Entity("Domain.Tasks.Event", b =>
                {
                    b.HasOne("Domain.Auth.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Tasks.Task", "Task")
                        .WithMany("Events")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Domain.Tasks.Label", b =>
                {
                    b.HasOne("Domain.Repositories.Repository", "Repository")
                        .WithMany("Labels")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Repository");
                });

            modelBuilder.Entity("Domain.Tasks.Task", b =>
                {
                    b.HasOne("Domain.Milestones.Milestone", "Milestone")
                        .WithMany("Tasks")
                        .HasForeignKey("MilestoneId");

                    b.HasOne("Domain.Repositories.Repository", "Repository")
                        .WithMany("Tasks")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Auth.User", "Creator")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Milestone");

                    b.Navigation("Repository");
                });

            modelBuilder.Entity("LabelTask", b =>
                {
                    b.HasOne("Domain.Tasks.Label", null)
                        .WithMany()
                        .HasForeignKey("LabelsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Tasks.Task", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RepositoryMemberTask", b =>
                {
                    b.HasOne("Domain.Repositories.RepositoryMember", null)
                        .WithMany()
                        .HasForeignKey("AssigneesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Tasks.Task", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RepositoryUser", b =>
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

            modelBuilder.Entity("RepositoryUser1", b =>
                {
                    b.HasOne("Domain.Auth.User", null)
                        .WithMany()
                        .HasForeignKey("WatchedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Repositories.Repository", null)
                        .WithMany()
                        .HasForeignKey("WatchedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Tasks.AssignEvent", b =>
                {
                    b.HasOne("Domain.Repositories.RepositoryMember", "Assignee")
                        .WithMany("AssignEvents")
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignee");
                });

            modelBuilder.Entity("Domain.Tasks.UnassignEvent", b =>
                {
                    b.HasOne("Domain.Repositories.RepositoryMember", "Assignee")
                        .WithMany("UnassignEvents")
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignee");
                });

            modelBuilder.Entity("Domain.Auth.User", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("OrganizationInvites");

                    b.Navigation("RepositoryInvites");

                    b.Navigation("SecondaryEmails");

                    b.Navigation("SocialAccounts");
                });

            modelBuilder.Entity("Domain.Milestones.Milestone", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Domain.Organizations.Organization", b =>
                {
                    b.Navigation("Members");

                    b.Navigation("PendingInvites");

                    b.Navigation("Repositories");
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
                    b.Navigation("Branches");

                    b.Navigation("Labels");

                    b.Navigation("Members");

                    b.Navigation("Milestones");

                    b.Navigation("PendingInvites");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Domain.Repositories.RepositoryMember", b =>
                {
                    b.Navigation("AssignEvents");

                    b.Navigation("UnassignEvents");
                });

            modelBuilder.Entity("Domain.Tasks.Task", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}