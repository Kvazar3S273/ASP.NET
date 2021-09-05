﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppSite.Areas.Admin.Models
{
    public class UserWithRoles
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
    }

    public class ChangeRoleModel
    {
        public string Role { get; set; }
        public string OldRole { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }

    }
}
