using System;
using System.Collections.Generic;

namespace DataCore.Models
{
    public partial class ProjectUsers
    {
        public int ProjectUserId { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }

        public Projects Project { get; set; }
        public Users User { get; set; }
    }
}
