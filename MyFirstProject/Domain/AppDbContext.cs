using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyFirstProject.Domain.Entities;

namespace MyFirstProject.Domain
{
    // Context of DB
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<ServiceCategory> ServiceCategories { get; set; } = null!;
        public DbSet<Service> Services { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            string adminName = "admin";
            string adminEmail = "admin@admin.com";
            string roleAdminId = "334893ff-8059-4490-8f89-82ea6c32884a";
            string userAdminId = "fc3e64e6-0c98-47ba-98ff-b8ec758c352b";

            // Adding role of site's admin
            builder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = roleAdminId,
                Name = adminName,
                NormalizedName = adminName.ToUpper(),
            });

            // adding new 'IdentityUser' as admin of the site
            builder.Entity<IdentityUser>().HasData(new IdentityUser()
            { 
                Id = userAdminId,
                UserName = adminName,
                NormalizedUserName = adminName.ToUpper(),
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToLower(),
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(new IdentityUser(), adminName),
                SecurityStamp = string.Empty,
                PhoneNumberConfirmed = true
            });

            // Определяем админа в соответствующую роль
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
            {
                RoleId = roleAdminId,
                UserId = userAdminId
            });
        }
    }
}
