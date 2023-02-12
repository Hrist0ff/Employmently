using employmently_be.Data;
using employmently_be.Data.Entities;
using employmently_be.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace employmently_be.DbContexts
{
    public class dbContext : IdentityDbContext<User>
    {
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ListingApplications> ListingApplications { get; set; }
        public DbSet<ForgotPassword> ForgotPasswords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "1", Name = "Administrator", NormalizedName = "ADMINISTRATOR".ToUpper() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2", Name = "Company", NormalizedName = "COMPANY".ToUpper() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "3", Name = "Candidate", NormalizedName = "CANDIDATE".ToUpper() });
            modelBuilder.Entity<User>()
                .ToTable("AspNetUsers");


            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = "1id", // primary key
                    UserName = "adminuser",
                    Email = "admin@admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    NormalizedUserName = "ADMINUSER",
                    PasswordHash = "AQAAAAEAACcQAAAAEDH4ryHkFVgvxLG8qcv5M79tg/UHQu2BbbVzQu92kgdh0lRMHHOuO1ywdDDDldRJHg==",
                    EmailConfirmed = true
                }
            ); ;
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>
               {
                   RoleId = "1",
                   UserId = "1id"
               }
            );
            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Python" },
                new Category() { Id = 2, Name = "Java" },
                new Category() { Id = 3, Name = "C++" },
                new Category() { Id = 4, Name = "C#" },
                new Category() { Id = 5, Name = "JavaScript" },
                new Category() { Id = 6, Name = "Ruby" },
                new Category() { Id = 7, Name = "Swift" },
                new Category() { Id = 8, Name = "Go" },
                new Category() { Id = 9, Name = "PHP" },
                new Category() { Id = 10, Name = "Kotlin" },
                new Category() { Id = 11, Name = "R" },
                new Category() { Id = 12, Name = "PERL" },
                new Category() { Id = 13, Name = "Assembly" },
                new Category() { Id = 14, Name = "Haskell" },
                new Category() { Id = 15, Name = "Elixir" },
                new Category() { Id = 16, Name = "C" }
            );
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source= (localdb)\\mssqllocaldb; Initial Catalog=Employmently;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
