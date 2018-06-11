using System.Collections.Generic;

namespace SupportWheel.Api.Models
{
    public class Engineer 
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual IList<Shift> Shifts { get; set; }
    }
}