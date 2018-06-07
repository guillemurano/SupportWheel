using System;
using System.Collections.Generic;
using System.Linq;
using SupportWheel.Api.Generics;
using SupportWheel.Api.Models;
using SupportWheel.Api.Repositories;

namespace SupportWheel.Api.Services 
{
    public class SchedulerService : ISchedulerService<Shift>
    {
        private IShiftRepository _schedulerRepository;

        public SchedulerService(IShiftRepository repository)
        {
            _schedulerRepository = repository;
        }

        public IList<Shift> Get(DateTime from, DateTime? to)
        {
            var endDate = to != null ? to.Value.Date.AddDays(1) : from.Date.AddDays(1);

            return _schedulerRepository.Get(
                s => s.Date.Date == from.Date && s.Date.Date < endDate).ToList();
        }

        public IList<Shift> Generate(DateTime from, DateTime? to)
        {
            _schedulerRepository.DeleteAll(s => s.IsDirty);
            //TODO: Add shift generation logic
            
            return new List<Shift>();
        }

        public void SaveShifts(bool approve)
        {
            if (approve)
            {
                _schedulerRepository.SaveAll(s => s.IsDirty);                
            }
            else
            {
                _schedulerRepository.DeleteAll(s => s.IsDirty);                
            }
        }
    }
}