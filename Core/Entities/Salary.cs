namespace Core.Entities
{
    public class Salary
    {
        public int UserId { get; set; }
        public User EmployeeUser { get; set; }
        public decimal Value { get; set; }
    }
}
