﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OnlineTimeTrack.Contexts;

namespace OnlineTimeTrack.Migrations
{
    [DbContext(typeof(OnlineTimeTrackContext))]
    [Migration("20191012115634_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OnlineTimeTrack.Models.Project", b =>
                {
                    b.Property<long>("ProjectID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("ProjectTitle");

                    b.Property<long?>("WorklogID");

                    b.HasKey("ProjectID");

                    b.HasIndex("WorklogID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("OnlineTimeTrack.Models.Timelog", b =>
                {
                    b.Property<long>("TimelogID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ActualWorkTimeEnd");

                    b.Property<DateTime>("ActualWorkTimeStart");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<long>("WorklogID");

                    b.HasKey("TimelogID");

                    b.HasIndex("WorklogID");

                    b.ToTable("Timelogs");
                });

            modelBuilder.Entity("OnlineTimeTrack.Models.User", b =>
                {
                    b.Property<long>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<int>("Age");

                    b.Property<string>("ContactNumber");

                    b.Property<DateTime>("Dob");

                    b.Property<string>("Email");

                    b.Property<string>("FullName");

                    b.Property<string>("Gender");

                    b.Property<string>("Password");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PasswordKey");

                    b.Property<string>("Token");

                    b.Property<string>("Username");

                    b.Property<long?>("WorklogID");

                    b.HasKey("UserID");

                    b.HasIndex("WorklogID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OnlineTimeTrack.Models.Worklog", b =>
                {
                    b.Property<long>("WorklogID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<DateTime>("DateAdded");

                    b.Property<DateTime>("DateModified");

                    b.Property<int>("EstimateWorkTime");

                    b.Property<string>("Feature");

                    b.Property<long>("ProjectID");

                    b.Property<long>("UserID");

                    b.HasKey("WorklogID");

                    b.ToTable("Worklogs");
                });

            modelBuilder.Entity("OnlineTimeTrack.Models.Project", b =>
                {
                    b.HasOne("OnlineTimeTrack.Models.Worklog", "Worklog")
                        .WithMany()
                        .HasForeignKey("WorklogID");
                });

            modelBuilder.Entity("OnlineTimeTrack.Models.Timelog", b =>
                {
                    b.HasOne("OnlineTimeTrack.Models.Worklog", "Worklog")
                        .WithMany("Timelogs")
                        .HasForeignKey("WorklogID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("OnlineTimeTrack.Models.User", b =>
                {
                    b.HasOne("OnlineTimeTrack.Models.Worklog", "Worklog")
                        .WithMany("Users")
                        .HasForeignKey("WorklogID");
                });
#pragma warning restore 612, 618
        }
    }
}
