namespace Core.ViewModels.AddressViewModels
{
    public class AddressViewModel
    {
        public int UserId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string? ZipCode { get; set; }
        public ushort? ApartmentNumber { get; set; }
    }
}