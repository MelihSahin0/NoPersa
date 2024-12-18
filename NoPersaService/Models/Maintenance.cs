﻿using System.ComponentModel.DataAnnotations;

namespace NoPersaService.Models
{
    public class Maintenance
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required string Type { get; set; }

        [Required]
        public required DateTime Date { get; set; }
    }
}
