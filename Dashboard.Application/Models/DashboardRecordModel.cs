using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Domain.Models
{
    public class DashboardRecordModel : BaseModel
    {
        public DateTime NextUpdate { get; set; }
        public string Name { get; set; }
        public string Cron { get; set; }
        public List<DashboardEventModel> Events { get; set; }
        public DashboardRecordStatus Status { get; set; }
    }
}
