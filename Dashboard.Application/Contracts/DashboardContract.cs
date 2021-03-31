using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Domain.Contracts
{
    public class CreateRecordContract
    {
        public string Name { get; set; }
        public string Cron { get; set; }
    }

    public class UpdateRecordContract : CreateRecordContract
    {
        public Guid Id { get; set; }
    }

    public class MakeReportContract
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DashboardEventType Type { get; set; }
    }

}