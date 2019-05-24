using System;
using System.Collections.Generic;

namespace DataCore.Models
{
    public partial class PermissionGroups
    {
        public PermissionGroups()
        {
            Roles = new HashSet<Roles>();
        }

        public int PermissionsId { get; set; }
        public string GroupName { get; set; }

        public ICollection<Roles> Roles { get; set; }
    }
}
