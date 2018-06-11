using System;

namespace SupportWheel.Api.Models
{
    public class Shift
    {
        public long Id { get; set; }

        public long EngineerId { get; set; }

        public DateTime Date { get; set; }

        public int Turn { get; set; }

        public virtual bool IsDirty { get; set; }

        public virtual Engineer Engineer { get; set; }
    }
}