using Crazy_zoo.Animals;
using Crazy_zoo.Modules;
using Microsoft.EntityFrameworkCore;

namespace Crazy_zoo
{
    public class CrazyZooContext : DbContext
    {
        public CrazyZooContext(DbContextOptions<CrazyZooContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>(b =>
            {
                b.ToTable("Animals");
                b.HasKey(a => a.Id);

                b.Property(a => a.Name).HasMaxLength(200).IsRequired();
                b.Property(a => a.Species).HasMaxLength(100).IsRequired();
                b.Property(a => a.Age).IsRequired();
                b.Property(a => a.EnclosureId).IsRequired(false);

                b.HasDiscriminator<string>("Discriminator")
                    .HasValue<Lion>(nameof(Lion))
                    .HasValue<Sheep>(nameof(Sheep))
                    .HasValue<Parrot>(nameof(Parrot))
                    .HasValue<Unicorn>(nameof(Unicorn))
                    .HasValue<Dragon>(nameof(Dragon))
                    .HasValue<Capybara>(nameof(Capybara))
                    .HasValue<Dolphin>(nameof(Dolphin))
                    .HasValue<Shark>(nameof(Shark))
                    .HasValue<Whale>(nameof(Whale))
                    .HasValue<CustomAnimal>(nameof(CustomAnimal));
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
