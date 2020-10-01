using Patron.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Patron.DAL
{
    public class PatronContext : DbContext
    {
        public PatronContext()
            : base("DefaultConnection")
        {

        }
        public DbSet<Models.Patron> Patrons { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Threshold> AuthorThresholds { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<CreditCard>()
                 .HasRequired(c => c.Patron)
                 .WithRequiredDependent(cc => cc.CreditCard);
                 
                 
            /*
            // Configure Student & StudentAddress entity
            modelBuilder.Entity<Models.Patron>()
                        .HasOptional(p => p.CreditCard) // Mark Address property optional in Student entity
                        .WithRequired(ad => ad.Patron); // mark Student property as required in StudentAddress entity. Cannot save StudentAddress without Student
*/

        }
    }
}