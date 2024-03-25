using Microsoft.EntityFrameworkCore;
using SmartCharginModels.Entities;

namespace SmartCharginRepository.DatabaseContext
{
    public class SmartCharginDbContext(DbContextOptions<SmartCharginDbContext> options) : DbContext(options)
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<ChargeStation> ChargeStations { get; set; }
        public DbSet<Connector> Connectors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasMany(g => g.ChargeStations)
                .WithOne(cs => cs.Group)
                .HasForeignKey(cs => cs.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChargeStation>()
                .HasOne(cs => cs.Group)
                .WithMany(g => g.ChargeStations)
                .HasForeignKey(cs => cs.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChargeStation>()
                .HasMany(cs => cs.Connectors)
                .WithOne(c => c.ChargeStation)
                .HasForeignKey(c => c.ChargeStationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChargeStation>()
                .HasMany(cs => cs.Connectors)
                .WithOne(c => c.ChargeStation)
                .HasForeignKey(c => c.ChargeStationId)
                .IsRequired()
                .HasConstraintName("ForeignKey_Connectors_ChargeStationId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Connector>()
                .HasOne(c => c.ChargeStation)
                .WithMany(cs => cs.Connectors)
                .HasForeignKey(c => c.ChargeStationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}