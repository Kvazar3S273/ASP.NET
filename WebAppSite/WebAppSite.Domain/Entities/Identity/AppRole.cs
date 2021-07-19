﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppSite.Domain.Entities.Identity
{
    public class AppRole : IdentityRole<long>
    {
        public virtual ICollection<AppUserRole> UserRole { get; set; }
    }
}