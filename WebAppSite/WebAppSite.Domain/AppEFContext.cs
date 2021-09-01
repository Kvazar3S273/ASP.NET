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
                        
            #endregion

            #region Catalog
            builder.ApplyConfiguration(new AnimalConfiguration());
            #endregion
        }
    }
}
