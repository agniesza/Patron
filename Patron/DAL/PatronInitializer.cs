using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Patron.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Patron.DAL
{
    public class PatronInitializer : DropCreateDatabaseIfModelChanges<PatronContext>
    {
        protected override void Seed(PatronContext context)
        {
            var categories = new List<Category>
            {
                new Category {Name = "Muzyka"},
                new Category {Name = "Malarstwo"},
                new Category {Name = "Rzeżba"},
                new Category {Name = "Taniec"},
                new Category {Name = "Fotografia"},
                 new Category {Name = "Sztuka"},
                new Category {Name = "Moda"},
                new Category {Name = "Teatr"},
                new Category {Name = "Film"},
                new Category {Name = "Grafika"},
                new Category {Name = "Publicystyka"},
                new Category {Name = "Design"},
                new Category {Name = "Rękodzieło"},
                new Category {Name = "Nauka"},
                new Category {Name = "Kulinaria"},
                new Category {Name = "Motoryzacja"},
                new Category {Name = "Polityka"},
                new Category {Name = "Zwierzęta"},
                new Category {Name = "Blog"},
                new Category {Name = "YouTube"},
                new Category {Name = "Podcast"},
                new Category {Name = "Cosplay"},
                new Category {Name = "Sport"},
                new Category {Name = "Podróże"},
                new Category {Name = "Gry"},
                new Category {Name = "Komiks"},
                new Category {Name = "Zdrowie"},
                new Category {Name = "Literatura"},
                new Category {Name = "Społeczne"},
                new Category {Name = "Inne"}
            };
            categories.ForEach(c => context.Categories.Add(c));
            context.SaveChanges();

            var patrons = new List<Models.Patron>
            {
                new Models.Patron {FirstName ="Julia", LastName ="Żaczek", UserName ="jula@wp.pl" },
                new Models.Patron {FirstName ="Jakub", LastName ="Rodak", UserName ="rod@wp.pl" },
                new Models.Patron {FirstName ="Eliasz", LastName ="Kula", UserName ="kula@wp.pl" }
         };
            patrons.ForEach(c => context.Patrons.Add(c));
            context.SaveChanges();

            var authors = new List<Author>
            {
                new Author {FirstName ="Darek", LastName ="Kurek", UserName ="kurek@wp.pl",
                Categories = new List<Category> {categories[0], categories[1]},
                    BankAccount="61921000080024460630000010",
                    Goals="Zakup nowego sprzętu i auta na koncerty",
                Description="Jestem muzykiem i chcę dawać radość ludziom",
                InstagramLink ="https://www.instagram.com/xxx/"},
                new Author {FirstName ="Maria", LastName ="Maria", UserName ="maria@wp.pl",
                Categories = new List<Category> {categories[3], categories[1]},
                    BankAccount="62921000080024460630000010",
                    Goals="Chcę zebrać fundusze na rozwijanie swoich umiejętności",
                Description="Maluję obrazy olejne",
                InstagramLink ="https://www.instagram.com/xxy/"}
         };
            authors.ForEach(c => context.Authors.Add(c));
            context.SaveChanges();

            var authorThresholds = new List<AuthorThreshold>
            {
                new AuthorThreshold
                {

                    Author = authors[0],
                    Patrons = new List<Models.Patron> {patrons[0], patrons[1]},
                    Name = "Super",
                    Description = "Dzięki! W podziękowaniu mogę zrobić ...",
                    MaxNumberOfPatrons = 30,
                    Value = 5
                },
                new AuthorThreshold
                {

                    Author =authors[0],
                    Patrons = new List<Models.Patron> {patrons[2], patrons[1]},
                    Name = "Jesteś mega!",
                    Description = "Dzięki! W podziękowaniu mogę zrobić ...",
                    MaxNumberOfPatrons=20,
                    Value=15
                },
                 new AuthorThreshold
                {

                    Author =authors[1],
                    Patrons = new List<Models.Patron> {patrons[2], patrons[0]},
                    Name = "Bosko",
                    Description = "Dzięki! W podziękowaniu mogę zrobić ...",
                    MaxNumberOfPatrons=50,
                    Value=10
                }
            };
            authorThresholds.ForEach(c => context.AuthorThresholds.Add(c));
            context.SaveChanges();

            var posts = new List<Post>
            {
                new Post
                {

                    Author = authors[0],
                    Patrons= new List<Models.Patron> {patrons[0], patrons[1], patrons[2]},
                    Content ="Testkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk",
                    Raiting=5.0,
                    NumberOfRatings=2,
                    Date=DateTime.Now

                },
            };
            posts.ForEach(c => context.Posts.Add(c));
            context.SaveChanges();

            var payments = new List<Payment>
            {
                new Payment
                {
                   Author=authors[0],
                    Patron=patrons[1],
                    //AuthorThreshold=authorThresholds[0],
                    
                   Value=authorThresholds[0].Value,
                  // Status= (Status) Enum.Parse(typeof(Status), "INACTIVE", true),
                   Periodicity=(Periodicity) Enum.Parse(typeof(Periodicity), "MONTHLY", true),
                   Date= DateTime.Today
    },
                                new Payment
                {
                   Author=authors[0],
                    Patron=patrons[1],
                    //AuthorThreshold=authorThresholds[0],
                    
                   Value=authorThresholds[0].Value,
                  // Status= (Status) Enum.Parse(typeof(Status), "ACTIVE", true),
                   Periodicity=(Periodicity) Enum.Parse(typeof(Periodicity), "MONTHLY", true),
                   Date= DateTime.Today
    },
                                                new Payment
                {
                    Author=authors[0],
                    Patron=patrons[1],
                    //AuthorThreshold=authorThresholds[1],
                    
                   Value=authorThresholds[1].Value,
                  // Status= (Status) Enum.Parse(typeof(Status), "ACTIVE", true),
                   Periodicity=(Periodicity) Enum.Parse(typeof(Periodicity), "ONE_TIME", true),
                   Date= DateTime.Today
    }
        };
            payments.ForEach(c => context.Payments.Add(c));
            context.SaveChanges();


            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            roleManager.Create(new IdentityRole("Admin"));

            var user = new ApplicationUser { UserName = "kokomonajlepsze@gmail.com" };
            string passwor = "Platon13";
            userManager.Create(user, passwor);

            userManager.AddToRole(user.Id, "Admin");

        }
    }
}