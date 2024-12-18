﻿using System.ComponentModel.DataAnnotations;

namespace NoPersaService.Models
{
    public class PortionSize
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required int Position { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required bool IsDefault { get; set; }

        public List<CustomersMenuPlan> CustomerMenuPlans { get; set; } = [];
    }
}
