using Dashboard.Application;
using Dashboard.Application.Services;
using Dashboard.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboradService _service;

        public DashboardController(ILogger<DashboardController> logger, IDashboradService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetRecords()
        {
           return Ok(await _service.GetRecordsAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AddRecord(CreateRecordContract contract)
        {
            await _service.CreateRecordAsync(contract);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> MakeReport(MakeReportContract contract)
        {
            await _service.MakeReportAsync(contract);
            return NoContent();
        }
    }
}
