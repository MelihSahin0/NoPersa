﻿using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class BoxStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int NumberOfBoxesPreviousDay { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int DeliveredBoxes {  get; set; }

        [Required]
        [IntType(min: 0)]
        public required int ReceivedBoxes { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int NumberOfBoxesCurrentDay { get; set; }

        public int CustomerId { get; set; }

        public required Customer Customer { get; set; }
    }
}
