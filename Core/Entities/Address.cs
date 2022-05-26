namespace Core.Entities
{
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public ushort? ApartmentNumber { get; set; }
        public string? ZipCode { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
