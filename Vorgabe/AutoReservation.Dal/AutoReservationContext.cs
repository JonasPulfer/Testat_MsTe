using System.Configuration;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace AutoReservation.Dal
{
    public class AutoReservationContext
        : DbContext
    {
        public static readonly LoggerFactory LoggerFactory = new LoggerFactory(
            new[] { new ConsoleLoggerProvider((_, logLevel) => logLevel >= LogLevel.Information, true) }
        );

        public DbSet<Auto> Autos { get; set; }

        public DbSet<Kunde> Kunden { get; set; }

        public DbSet<Reservation> Reservationen { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .EnableSensitiveDataLogging()
                    .UseLoggerFactory(LoggerFactory) // Warning: Do not create a new ILoggerFactory instance each time
                    .UseSqlServer(ConfigurationManager.ConnectionStrings[nameof(AutoReservationContext)].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
    

            modelbuilder.Entity<Kunde>()
                .HasKey(t => new { t.Id });

            modelbuilder.Entity<Kunde>()
                .Property(e => e.Vorname)
                .HasMaxLength(20)
                .IsRequired();

            modelbuilder.Entity<Kunde>()
                .Property(e => e.Nachname)
                .HasMaxLength(20)
                .IsRequired();

            modelbuilder.Entity<Kunde>()
                .Property(e => e.Geburtsdatum)
                .HasColumnType("datetime2(7)")
                .IsRequired();

            modelbuilder.Entity<Kunde>()
                .Property(e => e.RowVersion)
                .IsRowVersion();
            //.HasColumnType("timestamp");

            modelbuilder.Entity<Auto>()
                .HasKey(t => new { t.Id });

            modelbuilder.Entity<Auto>()
                .Property(e => e.Marke)
                .HasMaxLength(20)
                .IsRequired();

            modelbuilder.Entity<Auto>()
                .Property(e => e.Tagestarif)
                .HasColumnType("int")
                .IsRequired();

            modelbuilder.Entity<Auto>()
                .Property(e => e.RowVersion)
                .IsRowVersion();
            //.HasColumnType("timestamp");

            modelbuilder.Entity<Auto>()
                .Property(e => e.AutoKlasse)
                .HasColumnType("int")
                .IsRequired();

            modelbuilder.Entity<Auto>()
                .Property(e => e.Basistarif)
                .HasColumnType("int");

            modelbuilder.Entity<Reservation>()
                .HasKey(t => new { t.ReservationsNr });

            modelbuilder.Entity<Reservation>()
                .HasOne(k => k.Kunde)
                .WithMany(k => k.Reservationen)
                .HasForeignKey(d => d.KundeId)
                .IsRequired();

            modelbuilder.Entity<Reservation>()
                .HasOne(k => k.Auto)
                .WithMany(k => k.Reservationen)
                .HasForeignKey(d => d.AutoId)
                .IsRequired();

            modelbuilder.Entity<Reservation>()
                .Property(e => e.AutoId)
                .HasColumnType("int")
                .IsRequired();

            modelbuilder.Entity<Reservation>()
                .Property(e => e.KundeId)
                .HasColumnType("int")
                .IsRequired();

            modelbuilder.Entity<Reservation>()
                .Property(e => e.Von)
                .HasColumnType("datetime2(7)")
                .IsRequired();

            modelbuilder.Entity<Reservation>()
                .Property(e => e.Bis)
                .HasColumnType("datetime2(7)")
                .IsRequired();

            modelbuilder.Entity<Reservation>()
                .Property(e => e.RowVersion)
                .IsRowVersion();
                //.HasColumnType("timestamp");

            modelbuilder.Entity<Auto>()
                .HasDiscriminator<int>("AutoKlasse")
                //.HasValue<Auto>(0)
                .HasValue<StandardAuto>(1)
                .HasValue<LuxusklasseAuto>(2)
                .HasValue<MittelklasseAuto>(3);
        }
    }
}
