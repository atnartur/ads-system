﻿// <auto-generated />
using AdsSystem;
using AdsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace AdsSystem.Migrations
{
    [DbContext(typeof(Db))]
    [Migration("20171115114022_RemoveSizesFromBanner")]
    partial class RemoveSizesFromBanner
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("AdsSystem.Models.Banner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AuthorId");

                    b.Property<DateTime>("EndTime");

                    b.Property<string>("Html");

                    b.Property<string>("ImageFormat")
                        .HasMaxLength(10);

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsArchived");

                    b.Property<string>("Link")
                        .HasMaxLength(255);

                    b.Property<int>("MaxImpressions");

                    b.Property<string>("Name")
                        .HasMaxLength(255);

                    b.Property<int>("Priority");

                    b.Property<DateTime>("StartTime");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Banners");
                });

            modelBuilder.Entity("AdsSystem.Models.BannersZones", b =>
                {
                    b.Property<int>("BannerId");

                    b.Property<int>("ZoneId");

                    b.HasKey("BannerId", "ZoneId");

                    b.HasIndex("ZoneId");

                    b.ToTable("BannersZones");
                });

            modelBuilder.Entity("AdsSystem.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .HasMaxLength(255);

                    b.Property<string>("Pass")
                        .HasMaxLength(255);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AdsSystem.Models.View", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BannerId");

                    b.Property<DateTime>("Time");

                    b.Property<string>("UserAgent")
                        .HasMaxLength(255);

                    b.Property<int?>("ZoneId");

                    b.HasKey("Id");

                    b.HasIndex("BannerId");

                    b.HasIndex("ZoneId");

                    b.ToTable("Views");
                });

            modelBuilder.Entity("AdsSystem.Models.Zone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Height");

                    b.Property<string>("Name")
                        .HasMaxLength(255);

                    b.Property<int>("Width");

                    b.HasKey("Id");

                    b.ToTable("Zones");
                });

            modelBuilder.Entity("AdsSystem.Models.Banner", b =>
                {
                    b.HasOne("AdsSystem.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId");
                });

            modelBuilder.Entity("AdsSystem.Models.BannersZones", b =>
                {
                    b.HasOne("AdsSystem.Models.Banner", "Banner")
                        .WithMany("BannersZones")
                        .HasForeignKey("BannerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AdsSystem.Models.Zone", "Zone")
                        .WithMany("BannersZones")
                        .HasForeignKey("ZoneId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AdsSystem.Models.View", b =>
                {
                    b.HasOne("AdsSystem.Models.Banner", "Banner")
                        .WithMany()
                        .HasForeignKey("BannerId");

                    b.HasOne("AdsSystem.Models.Zone", "Zone")
                        .WithMany()
                        .HasForeignKey("ZoneId");
                });
#pragma warning restore 612, 618
        }
    }
}
