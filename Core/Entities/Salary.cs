namespace Core.Entities
{
    public class Salary
    {
        public int EmployeeId { get; set; }
        public User Employee { get; set; }
        public decimal Value { get; set; }
        public DateTime date { get; set; }
    }
}
