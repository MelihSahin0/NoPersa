﻿using System.ComponentModel.DataAnnotations;
using Website.Client.Models;

namespace Website.Client.FormModels
{
    public class Route
    {
        [ValidateComplexType]
        [Required]
        public required List<RouteOverview> RouteOverview { get; set; }
    }
}
