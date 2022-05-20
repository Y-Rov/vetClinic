namespace Core.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public Animal? Animal { get; set; }
    }
}
