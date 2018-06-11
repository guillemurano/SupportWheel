using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SupportWheel.Api.Models;
using SupportWheel.Api.Services;

namespace SupportWheel.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private readonly ISchedulerService _schedulerService;

        public SchedulerController(ISchedulerService service)
        {
            _schedulerService = service;
        }

        /// <summary>
        /// Get shifts for given timelapse
        /// </summary>
        /// <param name="from">Start Date</param>
        /// <param name="to">End Date (null for today)</param>
        /// <returns>List of shifts for given dates</returns>
        [HttpGet]
        public ActionResult<List<Shift>> GetCustomSchedule(DateTime from, DateTime? to = null)
        {   
            if ((to.HasValue && from > to.Value) || (!to.HasValue && from > DateTime.Today))
                return BadRequest("From 'date' cannot be greater than 'to' Date.");

            var schedule = _schedulerService.Get(from, to);

            return Ok(schedule);
        }

        /// <summary>
        /// Get preliminary shifts for given date
        /// </summary>
        /// <param name="to">End Date (null for today)</param>
        /// <returns>List of shifts from today to given date.</returns>
        [HttpPost]
        public ActionResult<List<Shift>> GenerateShifts(DateTime? to = null)
        {
            if (to.HasValue && to.Value < DateTime.Today)
                return BadRequest("Cannot generate shifts for dates previous than today.");

            var schedule = _schedulerService.Generate(to);
            return Ok(schedule);
        }

        /// <summary>
        /// Approve pending shifts
        /// </summary>
        /// <param name="approve">Bool for approval</param>
        /// <returns>Ok status</returns>
        [HttpPost]
        public IActionResult SaveGeneratedShifts(bool approve)
        {
            _schedulerService.SaveShifts(approve);
            return Ok();
        }
    }
}