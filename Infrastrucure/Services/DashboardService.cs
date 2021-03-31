using Dashboard.Application;
using Dashboard.Application.Repository;
using Dashboard.Application.Services;
using Dashboard.Domain;
using Dashboard.Domain.Contracts;
using Dashboard.Domain.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Infrastructure
{
    public class DashboardService : IDashboradService
    {
        private readonly IRepository<DashboardRecordModel> _recordsRepository;

        public DashboardService(IRepository<DashboardRecordModel> recordRepository)
        {
            _recordsRepository = recordRepository;
        }

        public async Task CreateRecordAsync(CreateRecordContract contract)
        {

            var createdEvent = new DashboardEventModel
            {
                Created = DateTime.UtcNow,
                Description = $"{contract.Name} created with cron {contract.Cron}",
                Id = Guid.NewGuid(),
                Type = DashboardEventType.Created
            };

            var record = new DashboardRecordModel
            {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Events = new List<DashboardEventModel>() { createdEvent },
                Name = contract.Name,
                NextUpdate = GetNextUpdate(contract.Cron, DateTime.UtcNow),
                Cron = contract.Cron,
                Status  = DashboardRecordStatus.NotStarted
            };

            await _recordsRepository.SaveAsync(record);
        }

        public async Task DeleteRecordAsync(Guid id)
        {
           await _recordsRepository.RemoveAsync(id);
        }

        public async Task<DashboardRecordModel> GetRecordAsync(string name)
        {
            await Task.CompletedTask;
            return _recordsRepository.AsQueryable().FirstOrDefault(x => x.Name == name);
        }

        public async Task<DashboardRecordModel> GetRecordAsync(Guid id)
        {
            return await _recordsRepository.GetOneAsync(id);
        }

        public async Task<IEnumerable<DashboardRecordModel>> GetRecordsAsync()
        {
            return await _recordsRepository.GetAllAsync();
        }

        public async Task MakeReportAsync(MakeReportContract contract)
        {
            var record = await GetRecordAsync(contract.Name);
            record.Events.Add(new DashboardEventModel
            {
                Created = DateTime.UtcNow,
                Description = contract.Description,
                Id = Guid.NewGuid(),
                Type = contract.Type
            });

            record.Status = contract.Type switch
            {
                DashboardEventType.Started => DashboardRecordStatus.Started,
                DashboardEventType.Completed => DashboardRecordStatus.Completed,
                DashboardEventType.InProggress => DashboardRecordStatus.Working,
                DashboardEventType.ExceptionRaised => DashboardRecordStatus.FailedWithException,
                DashboardEventType.ScheduleCheckFail => DashboardRecordStatus.NotStarted,
                _ => record.Status
            };

            record.NextUpdate = GetNextUpdate(record.Cron, DateTime.UtcNow);
            record.Updated = DateTime.UtcNow;
            await _recordsRepository.SaveAsync(record);
        }

        public async Task UpdateRecordAsync(UpdateRecordContract contract)
        {
            var record = await _recordsRepository.GetOneAsync(contract.Id);
            record.Name = contract.Name;
            record.Cron = contract.Cron;
            await _recordsRepository.SaveAsync(record);
        }

        private DateTime GetNextUpdate(string cron, DateTime date)
        { 
            var expression = new CronExpression(cron);
            return expression.GetTimeAfter(date).Value.DateTime;
        }
    }
}
