using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SupportWheel.Api.Models;
using SupportWheel.Api.Services;

namespace SupportWheel.Api.Controllers
{
    [Route("api/Shedule")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly ISchedulerService<Shift> _schedulerService;

        public SchedulerController(ISchedulerService<Shift> service)
        {
            _schedulerService = service;
        }

        [HttpGet]
        public ActionResult<List<Shift>> GetCustomSchedule(DateTime from, DateTime? to)
        {
            var schedule = _schedulerService.Get(from, to);

            return Ok(schedule);
        }

        [HttpGet]
        public ActionResult<List<Shift>> GenerateShifts(DateTime from, DateTime? to)
        {
            var schedule = _schedulerService.Generate(from, to);
            return Ok(schedule);
        }

        [HttpPost]
        public IActionResult SaveGeneratedShifts(bool approve)
        {
            _schedulerService.SaveShifts(approve);
            return Ok();
        }
    }
}