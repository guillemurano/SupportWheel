using System;
using System.Collections.Generic;
using SupportWheel.Api.Generics;
using SupportWheel.Api.Models;

namespace SupportWheel.Api.Services
{
    public interface ISchedulerService
    {
        IList<Shift> Get(DateTime from, DateTime? to);

        IList<Shift> Generate(DateTime? to);
        
        void SaveShifts(bool approve);
    }
}