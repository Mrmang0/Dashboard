using Dashboard.Domain.Contracts;
using Dashboard.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Application.Services
{
    public interface IDashboradService
    {
        Task<IEnumerable<DashboardRecordModel>> GetRecordsAsync();
        Task<DashboardRecordModel> GetRecordAsync(string name);
        Task<DashboardRecordModel> GetRecordAsync(Guid id);
        Task CreateRecordAsync(CreateRecordContract contract);
        Task UpdateRecordAsync(UpdateRecordContract contract);
        Task DeleteRecordAsync(Guid id);
        Task MakeReportAsync(MakeReportContract contract);
    }
}
