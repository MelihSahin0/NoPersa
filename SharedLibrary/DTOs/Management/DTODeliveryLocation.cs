﻿
namespace SharedLibrary.DTOs.Management
{
    public class DTODeliveryLocation
    {
        public int Id { get; set; }

        public string? Address { get; set; }

        public string? Region { get; set; }

        public string? GeoLocation { get; set; }

        public string? DeliveryWhishes { get; set; }
    }
}
