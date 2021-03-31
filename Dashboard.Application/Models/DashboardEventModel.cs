using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Domain.Models
{
    public class DashboardEventModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public DashboardEventType Type { get; set; }
    }
}
