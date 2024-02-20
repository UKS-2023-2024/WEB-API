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
    [Migration("20231105181943_OrgRoleAndPermissions")]
    partial class OrgRoleAndPermissions
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
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationRole", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("OrganizationRoles");
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

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("OwnerId");

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

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("RepositoryId");

                    b.ToTable("RepositoryMembers");
                });

            modelBuilder.Entity("OrganizationPermissionOrganizationRole", b =>
                {
                    b.Property<string>("PermissionsValue")
                        .HasColumnType("text");

                    b.Property<string>("RolesName")
                        .HasColumnType("text");

                    b.HasKey("PermissionsValue", "RolesName");

                    b.HasIndex("RolesName");

                    b.ToTable("OrganizationPermissionOrganizationRole");
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

            modelBuilder.Entity("Domain.Repositories.Repository", b =>
                {
                    b.HasOne("Domain.Organizations.Organization", "Organization")
                        .WithMany()
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Auth.User", "Owner")
                        .WithMany("OwnedRepositories")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("Owner");
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

            modelBuilder.Entity("OrganizationPermissionOrganizationRole", b =>
                {
                    b.HasOne("Domain.Organizations.OrganizationPermission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsValue")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Organizations.OrganizationRole", null)
                        .WithMany()
                        .HasForeignKey("RolesName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("Domain.Auth.User", b =>
                {
                    b.Navigation("OwnedRepositories");

                    b.Navigation("SecondaryEmails");

                    b.Navigation("SocialAccounts");
                });

            modelBuilder.Entity("Domain.Organizations.Organization", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("Domain.Organizations.OrganizationRole", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("Domain.Repositories.Repository", b =>
                {
                    b.Navigation("Members");
                });
#pragma warning restore 612, 618
        }
    }
}
