using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Service
{
    public class CreditCardContext : DbContext
    {

        private readonly IConfiguration Configuration;

        #region Constructors

        public CreditCardContext(IConfiguration configuration, DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
            Configuration = configuration;
        }

        #endregion

        #region Properties

        public DbSet<CardActivity> CardActivity { get; set; }

        #endregion

        #region Overridden Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CardActivity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.Category);
                entity.Property(e => e.Type).IsRequired();
            });
        }

        #endregion

    }
}
