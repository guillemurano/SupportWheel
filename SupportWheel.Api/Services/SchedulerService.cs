using System;
using System.Collections.Generic;
using System.Linq;
using SupportWheel.Api.Constants;
using SupportWheel.Api.Generics;
using SupportWheel.Api.Models;
using SupportWheel.Api.Repositories;

namespace SupportWheel.Api.Services 
{
    public class SchedulerService : ISchedulerService
    {
        private IShiftRepository _schedulerRepository;

        public SchedulerService(IShiftRepository repository)
        {
            _schedulerRepository = repository;
        }

        public IList<Shift> GetAll()
        {
            return _schedulerRepository.Get();
        }

        /// <summary>
        /// Get all shifts for the given time lapse
        /// </summary>
        /// <param name="from">Start Date</param>
        /// <param name="to">End date (null for today)</param>
        /// <returns>List of shifts</returns>
        public IList<Shift> Get(DateTime from, DateTime? to)
        {
            var endDate = to != null ? to.Value.Date.AddDays(1) : from.Date.AddDays(1);

            return _schedulerRepository.Get(
                s => s.Date.Date == from.Date && s.Date.Date < endDate).ToList();
        }

        /// <summary>
        /// Generates shifts up to given date or Today if null
        /// </summary>
        /// <param name="to">End date to generate shifts (null for today)</param>
        /// <returns></returns>
        public IList<Shift> Generate(DateTime? to)
        {
            var shifts = new List<Shift>();

            _schedulerRepository.DeleteAll(s => s.IsDirty);
            
            var endDate = to.HasValue ? to.Value : DateTime.Today;
            //var startDate =  endDate.AddDays(Constants.DAYS_UNREPEATED_SHIFT * (-1));
            var startDate =  endDate.AddDays(-14);

            var currentShifts = _schedulerRepository.Get(s => s.Date < endDate && s.Date >= startDate)
                        .GroupBy(s => s.EngineerId)
                        .Select(g => new { EngineerId = g.Key, Turns = g.Sum(i => i.Turn) 
                    });

            for(var i = 0; i < (endDate - DateTime.Today).TotalDays; i++)
            {
                var pickable = currentShifts.Where(s => s.Turns < 2);
                var random = new Random();
                var picked = pickable.ElementAt(random.Next(pickable.Count()));
                var shiftDate = DateTime.Today.AddDays(i);
                
                shifts.Add( new Shift() {
                    Date = shiftDate,
                    EngineerId = picked.EngineerId,
                    Turn = shifts.Any(s => s.Date == shiftDate) ? 1 : 2
                });
            }

            return shifts;
        }

        /// <summary>
        /// Approve or disaprove all proposed shifts
        /// </summary>
        /// <param name="approve">Shifts approval</param>
        public void SaveShifts(bool approve)
        {
            if (approve)
            {
                _schedulerRepository.AcceptAll(s => s.IsDirty == approve);                
            }
            else
            {
                _schedulerRepository.DeleteAll(s => s.IsDirty == !approve);                
            }
        }
    }
}