using domain;
using Microsoft.EntityFrameworkCore;

namespace service
{
    public class CreditCardContext : DbContext
    {

        #region Constructors

        public CreditCardContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
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
