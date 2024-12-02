using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NailApp.Models;

namespace NailApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<NailStylist> NailStylists { get; set; }
        public DbSet<Agenda> Agendas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade NailStylist
            modelBuilder.Entity<NailStylist>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Name).IsRequired().HasMaxLength(100);
                entity.Property(n => n.IsAvailable).HasDefaultValue(true);
            });

            // Configuração da entidade Agenda
            modelBuilder.Entity<Agenda>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Date).IsRequired();
                entity.Property(a => a.Time).IsRequired();

                entity.HasOne(a => a.NailStylist)
                    .WithMany(n => n.Agendas)
                    .HasForeignKey(a => a.NailStylistId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Client)
                    .WithMany()
                    .HasForeignKey(a => a.ClientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}