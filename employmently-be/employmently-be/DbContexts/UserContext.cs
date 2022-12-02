using employmently_be.Data;
using employmently_be.Data.Entities;
using employmently_be.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace employmently_be.DbContexts
{
    public class UserContext : IdentityDbContext<User>
    {
        public DbSet<Listing> Listings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "0", Name = "Administrator", NormalizedName = "ADMINISTRATOR".ToUpper() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "1", Name = "Company", NormalizedName = "COMPANY".ToUpper() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2", Name = "Candidate", NormalizedName = "CANDIDATE".ToUpper() });
            modelBuilder.Entity<User>()
                .ToTable("AspNetUsers");
            


            modelBuilder.Entity<User>().HasData(
                new User    
                {
                    Id = "0", // primary key
                    UserName = "adminuser",
                    Email = "admin@admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM",
                    NormalizedUserName = "ADMINUSER",
                    PasswordHash = "admin123"
                },
                new User
                {
                    Id = "1",
                    UserName = "Hri",
                    NormalizedUserName = "HRI",
                    PasswordHash = "Pa$$w0rd"
                }
            );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
               new IdentityUserRole<string>
               {
                   RoleId = "0",
                   UserId = "0"
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
