﻿namespace Core.ViewModels.AddressViewModels
{
    public class AddressCreateReadViewModel
    {
        public int Id { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? House { get; set; }
        public string? ZipCode { get; set; }
        public ushort? ApartmentNumber { get; set; }
    }
}