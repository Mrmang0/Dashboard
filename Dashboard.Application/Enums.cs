using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Domain
{
    public enum DashboardEventType
    {
        Created,
        Updated,
        Started,
        InProggress,
        Completed,
        ExceptionRaised,
        ScheduleCheckFail
    }

    public enum DashboardRecordStatus
    {
        Started,
        NotStarted,
        Completed,
        Working,
        FailedWithException,
    }
}
