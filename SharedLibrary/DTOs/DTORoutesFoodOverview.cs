﻿
namespace SharedLibrary.DTOs
{
    public class DTORoutesFoodOverview
    {
        public string? Name { get; set; }

        public int Position { get; set; }

        public List<DTOCustomersFood>? CustomersFoods { get; set; }
    }
}
