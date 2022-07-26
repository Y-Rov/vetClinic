namespace Core.Models.Finance
{
    public class SalaryDetail
    {
        public int Days { get; set; }
        public decimal Value { get; set; }
        public SalaryDetail(int days, decimal value)
        {
            Days = days;
            Value = value;
        }
    }
}
