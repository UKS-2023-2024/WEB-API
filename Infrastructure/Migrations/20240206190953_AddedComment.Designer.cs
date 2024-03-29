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
    [Migration("20240206190953_AddedComment")]
    partial class AddedComment
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
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
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

                    b.Property<string>("GitToken")
                        .HasColumnType("text");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<int>("NotificationPreferences")
                        .HasColumnType("integer");

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

            modelBuilder.Entity("Domain.Comments.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("TaskId");

                    b.ToTable("Comments");
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

            modelBuilder.Entity("Domain.Notifications.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
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

                    b.Property<int?>("memberTeamId")
                        .HasColumnType("integer");

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

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("OrganizationId", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("OrganizationMembers");
                });

            modelBuilder.Entity("Domain.Repositories.Repository", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HttpCloneUrl")
                        .HasColumnType("text");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("OrganizationId")
                        .HasColumnType("uuid");

                    b.Property<string>("SshCloneUrl")
                        .HasColumnType("text");

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

            modelBuilder.Entity("Domain.Repositories.RepositoryWatcher", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("RepositoryId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<int>("WatchingPreferences")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RepositoryId");

                    b.HasIndex("UserId");

                    b.ToTable("RepositoryWatchers");
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

            modelBuilder.Entity("Domain.Tasks.AssignEvent", b =>
                {
                    b.HasBaseType("Domain.Tasks.Event");

                    b.Property<Guid>("AssigneeId")
                        .HasColumnType("uuid");

                    b.HasIndex("AssigneeId");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Domain.Tasks.Interfaces.AssignMilestoneEvent", b =>
                {
                    b.HasBaseType("Domain.Tasks.Event");

                    b.Property<Guid>("MilestoneId")
                        .HasColumnType("uuid");

                    b.HasIndex("MilestoneId");

                    b.HasDiscriminator().HasValue(4);
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

            modelBuilder.Entity("Domain.Tasks.UnassignMilestoneEvent", b =>
                {
                    b.HasBaseType("Domain.Tasks.Event");

                    b.Property<Guid?>("MilestoneId")
                        .HasColumnType("uuid");

                    b.HasIndex("MilestoneId");

                    b.ToTable("Events", t =>
                        {
                            t.Property("MilestoneId")
                                .HasColumnName("UnassignMilestoneEvent_MilestoneId");
                        });

                    b.HasDiscriminator().HasValue(5);
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
                    b.HasOne("Domain.Auth.User", "User")
                        .WithMany("SocialAccounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
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

            modelBuilder.Entity("Domain.Comments.Comment", b =>
                {
                    b.HasOne("Domain.Auth.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Tasks.Task", "Task")
                        .WithMany("Comments")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Task");
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

            modelBuilder.Entity("Domain.Notifications.Notification", b =>
                {
                    b.HasOne("Domain.Auth.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
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

                    b.Navigation("Member");

                    b.Navigation("Organization");
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

            modelBuilder.Entity("Domain.Repositories.RepositoryWatcher", b =>
                {
                    b.HasOne("Domain.Repositories.Repository", "Repository")
                        .WithMany("WatchedBy")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Auth.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Repository");

                    b.Navigation("User");
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

            modelBuilder.Entity("Domain.Tasks.AssignEvent", b =>
                {
                    b.HasOne("Domain.Repositories.RepositoryMember", "Assignee")
                        .WithMany("AssignEvents")
                        .HasForeignKey("AssigneeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignee");
                });

            modelBuilder.Entity("Domain.Tasks.Interfaces.AssignMilestoneEvent", b =>
                {
                    b.HasOne("Domain.Milestones.Milestone", "Milestone")
                        .WithMany()
                        .HasForeignKey("MilestoneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Milestone");
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

            modelBuilder.Entity("Domain.Tasks.UnassignMilestoneEvent", b =>
                {
                    b.HasOne("Domain.Milestones.Milestone", "Milestone")
                        .WithMany()
                        .HasForeignKey("MilestoneId");

                    b.Navigation("Milestone");
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

            modelBuilder.Entity("Domain.Repositories.Repository", b =>
                {
                    b.Navigation("Branches");

                    b.Navigation("Labels");

                    b.Navigation("Members");

                    b.Navigation("Milestones");

                    b.Navigation("PendingInvites");

                    b.Navigation("Tasks");

                    b.Navigation("WatchedBy");
                });

            modelBuilder.Entity("Domain.Repositories.RepositoryMember", b =>
                {
                    b.Navigation("AssignEvents");

                    b.Navigation("UnassignEvents");
                });

            modelBuilder.Entity("Domain.Tasks.Task", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
