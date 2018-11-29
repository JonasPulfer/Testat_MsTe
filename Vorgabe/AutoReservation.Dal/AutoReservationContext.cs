using System.Configuration;
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

        protected override void OnModelCreating(Modelbuilder modelbuilder)
        {

            modelbuilder.Entity<Kunde>()
                .HasKey(t => new { t.Id })
                .Property(e => e.Vorname)
                .HasColumnType("NVARCHAR(20)")
                .IsRequired()
                .Property(e => e.Nachname)
                .HasColumnType("NVARCHAR(20)")
                .IsRequired()
                .Property(e => e.Geburtsdatum)
                .HasColumnType("datetime2(7)")
                .IsRequired()
                .Property(e => e.RowVersion)
                .HasColumnType("timestamp");

            modelbuilder.Entity<Auto>()
                .HasKey(t => new { t.Id })
                .Property(e => e.MARKE)
                .HasColumnType("NVARCHAR(20)")
                .IsRequired()
                .Property(e => e.Tagestarif)
                .HasColumnType("int")
                .IsRequired()
                .Property(e => e.RowVersion)
                .HasColumnType("timestamp")
                .Property(e => e.AutoKlasse)
                .HasColumnType("int")
                .IsRequired()
                .Property(e => e.Basistarif)
                .HasColumnType("int");

            modelbuilder.Entity<Reservation>()
                .HashKey(t => new { t.ReservationsNr })
                .Property(e => e.AutoId)
                .HasColumnType("int")
                .isRequired()
                 .Property(e => e.KundeId)
                .HasColumnType("int")
                .isRequired()
                .Property(e => e.Von)
                .HasColumnType("datetime2(7)")
                .isRequired()
                .Property(e => e.Bis)
                .HasColumnType("datetime2(7)")
                .isRequired()
                .Property(e => e.RowVersion)
                .HasColumnType("timestamp");




        }
    }
}
