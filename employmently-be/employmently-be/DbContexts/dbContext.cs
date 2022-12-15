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
                    Id = "1", // primary key
                    UserName = "adminuser",
                    Email = "admin@admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    NormalizedUserName = "ADMINUSER",
                    PasswordHash = "AQAAAAEAACcQAAAAEDH4ryHkFVgvxLG8qcv5M79tg/UHQu2BbbVzQu92kgdh0lRMHHOuO1ywdDDDldRJHg=="
                    // Admin123@
                }
            ) ;
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>
               {
                   RoleId = "1",
                   UserId = "1"
               }
            );
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source= (localdb)\\mssqllocaldb; Initial Catalog=Employmently;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
