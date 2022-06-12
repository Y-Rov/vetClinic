namespace Core.Entities
{
    public class Salary
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public User Employee { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
