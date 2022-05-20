namespace Core.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int ZipCode { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }
        public byte? BuildingNumber { get; set; }
        public ushort? FlatNumber { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
