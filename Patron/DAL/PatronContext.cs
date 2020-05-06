using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Patron.Models;

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
        public DbSet<AuthorThreshold> AuthorThresholds { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}