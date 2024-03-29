﻿// <auto-generated />
using System;
using Infrastructure.DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.DataAccess.Migrations
{
    [DbContext(typeof(WeatherDbContext))]
    [Migration("20190817184306_CreateInitial")]
    partial class CreateInitial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Domain.Entities.Forecast", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<long>("GeoObjectId");

                    b.Property<double>("MaxTemperature");

                    b.Property<double>("MinTemperature");

                    b.Property<double>("Precipitation");

                    b.HasKey("Id");

                    b.HasIndex("GeoObjectId");

                    b.ToTable("Forecasts");
                });

            modelBuilder.Entity("Domain.Entities.GeoObject", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("GeoObjects");
                });

            modelBuilder.Entity("Domain.Entities.Forecast", b =>
                {
                    b.HasOne("Domain.Entities.GeoObject", "GeoObject")
                        .WithMany()
                        .HasForeignKey("GeoObjectId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
