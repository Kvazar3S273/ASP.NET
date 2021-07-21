using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAppSite.Domain.Configuration.Catalog;
using WebAppSite.Domain.Configuration.Identity;
using WebAppSite.Domain.Entities.Catalog;
using WebAppSite.Domain.Entities.Identity;

namespace WebAppSite.Domain
{
    public class AppEFContext : IdentityDbContext<AppUser, AppRole, long, IdentityUserClaim<long>,
                                            AppUserRole, IdentityUserLogin<long>,
                                            IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public AppEFContext(DbContextOptions<AppEFContext> options)
            : base(options)
        {

        }
        public DbSet<Animal> Animals { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Identity
            builder.ApplyConfiguration(new AppUserRoleConfiguration());

            //builder.Entity<AppUserRole>(userRole =>
            //{
            //    userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            //    userRole.HasOne(ur => ur.Role)
            //        .WithMany(r => r.UserRoles)
            //        .HasForeignKey(ur => ur.RoleId)
            //        .IsRequired();

            //    userRole.HasOne(ur => ur.User)
            //        .WithMany(r => r.UserRoles)
            //        .HasForeignKey(ur => ur.UserId)
            //        .IsRequired();
            //});
            #endregion

            #region Catalog
            builder.ApplyConfiguration(new AnimalConfiguration());
            #endregion
        }
    }
}
