using System;
using Intersect.Server.Database.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Intersect.Server.Migrations.MySql.Logging
{
    [DbContext(typeof(MySqlLoggingContext))]
    [Migration("20231126092510_AddingNations")]
    partial class AddingNations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Intersect.Server.Database.Logging.Entities.ChatHistory", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<string>("Ip")
                    .HasColumnType("longtext");

                b.Property<string>("MessageText")
                    .HasColumnType("longtext");

                b.Property<int>("MessageType")
                    .HasColumnType("int");

                b.Property<Guid>("PlayerId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<Guid>("TargetId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<DateTime>("TimeStamp")
                    .HasColumnType("datetime(6)");

                b.Property<Guid>("UserId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.HasKey("Id");

                b.ToTable("ChatHistory");
            });

            modelBuilder.Entity("Intersect.Server.Database.Logging.Entities.GuildHistory", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<Guid>("GuildId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<Guid>("InitiatorId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<string>("Ip")
                    .HasColumnType("longtext");

                b.Property<string>("Meta")
                    .HasColumnType("longtext");

                b.Property<Guid>("PlayerId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<DateTime>("TimeStamp")
                    .HasColumnType("datetime(6)");

                b.Property<int>("Type")
                    .HasColumnType("int");

                b.Property<Guid>("UserId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.HasKey("Id");

                b.ToTable("GuildHistory");
            });

            modelBuilder.Entity("Intersect.Server.Database.Logging.Entities.NationHistory", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<Guid>("InitiatorId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<string>("Ip")
                    .HasColumnType("longtext");

                b.Property<string>("Meta")
                    .HasColumnType("longtext");

                b.Property<Guid>("NationId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<Guid>("PlayerId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<DateTime>("TimeStamp")
                    .HasColumnType("datetime(6)");

                b.Property<int>("Type")
                    .HasColumnType("int");

                b.Property<Guid>("UserId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.HasKey("Id");

                b.ToTable("NationHistory");
            });

            modelBuilder.Entity("Intersect.Server.Database.Logging.Entities.TradeHistory", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<string>("Ip")
                    .HasColumnType("longtext");

                b.Property<string>("ItemsJson")
                    .HasColumnType("longtext")
                    .HasColumnName("Items");

                b.Property<Guid>("PlayerId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<Guid>("TargetId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<string>("TargetItemsJson")
                    .HasColumnType("longtext")
                    .HasColumnName("TargetItems");

                b.Property<DateTime>("TimeStamp")
                    .HasColumnType("datetime(6)");

                b.Property<Guid>("TradeId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<Guid>("UserId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.HasKey("Id");

                b.ToTable("TradeHistory");
            });

            modelBuilder.Entity("Intersect.Server.Database.Logging.Entities.UserActivityHistory", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<int>("Action")
                    .HasColumnType("int");

                b.Property<string>("Ip")
                    .HasColumnType("longtext");

                b.Property<string>("Meta")
                    .HasColumnType("longtext");

                b.Property<int>("Peer")
                    .HasColumnType("int");

                b.Property<Guid?>("PlayerId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.Property<DateTime>("TimeStamp")
                    .HasColumnType("datetime(6)");

                b.Property<Guid>("UserId")
                    .HasColumnType("char(36)")
                    .UseCollation("ascii_general_ci");

                b.HasKey("Id");

                b.ToTable("UserActivityHistory");
            });

            modelBuilder.Entity("Intersect.Server.Database.Logging.RequestLog", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("char(36)")
                    .HasColumnOrder(0)
                    .UseCollation("ascii_general_ci");

                b.Property<byte>("Level")
                    .HasColumnType("tinyint unsigned");

                b.Property<string>("Method")
                    .HasColumnType("longtext");

                b.Property<string>("SerializedRequestHeaders")
                    .HasColumnType("longtext")
                    .HasColumnName("RequestHeaders");

                b.Property<string>("SerializedResponseHeaders")
                    .HasColumnType("longtext")
                    .HasColumnName("ResponseHeaders");

                b.Property<int>("StatusCode")
                    .HasColumnType("int");

                b.Property<string>("StatusMessage")
                    .HasColumnType("longtext");

                b.Property<DateTime>("Time")
                    .HasColumnType("datetime(6)");

                b.Property<string>("Uri")
                    .HasColumnType("longtext");

                b.HasKey("Id");

                b.ToTable("RequestLogs");
            });
#pragma warning restore 612, 618
        }
    }
}