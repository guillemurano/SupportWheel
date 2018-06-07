namespace SupportWheel.Api.Models
{
    public class Engineer 
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual Shift Shift { get; set; }
    }
}