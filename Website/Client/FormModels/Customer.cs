using System.ComponentModel.DataAnnotations;
using Website.Client.Exceptions;
using Website.Client.Util;
using Website.Client.Validations;

namespace Website.Client.FormModels
{
    public class Customer
    {
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? SerialNumber { get; set; }

        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Address { get; set; }

        [Required(ErrorMessage = "Region is required.")]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Region { get; set; }

        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? GeoLocation { get; set; }

        [Required(ErrorMessage = "Month is required.")]
        [ValidateComplexType]
        public required MonthOfTheYear Month { get; set; }

        [Required]
        public required int Article { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public required double Price { get; set; }

        [Required(ErrorMessage = "Monthly Delivery is required")]
        [ValidateComplexType]
        public required MonthlyDelivery MonthlyDeliveries { get; set; }

        [Required]
        public Weekdays? Workdays { get; set; }

        [Required]
        public Weekdays? Holidays { get; set; }

        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? ContactInformation { get; set; }

        public DailyDelivery GetDailyDeliveryByDay(int day)
        {
            return day switch
            {
                1 => MonthlyDeliveries.Day1,
                2 => MonthlyDeliveries.Day2,
                3 => MonthlyDeliveries.Day3,
                4 => MonthlyDeliveries.Day4,
                5 => MonthlyDeliveries.Day5,
                6 => MonthlyDeliveries.Day6,
                7 => MonthlyDeliveries.Day7,
                8 => MonthlyDeliveries.Day8,
                9 => MonthlyDeliveries.Day9,
                10 => MonthlyDeliveries.Day10,
                11 => MonthlyDeliveries.Day11,
                12 => MonthlyDeliveries.Day12,
                13 => MonthlyDeliveries.Day13,
                14 => MonthlyDeliveries.Day14,
                15 => MonthlyDeliveries.Day15,
                16 => MonthlyDeliveries.Day16,
                17 => MonthlyDeliveries.Day17,
                18 => MonthlyDeliveries.Day18,
                19 => MonthlyDeliveries.Day19,
                20 => MonthlyDeliveries.Day20,
                21 => MonthlyDeliveries.Day21,
                22 => MonthlyDeliveries.Day22,
                23 => MonthlyDeliveries.Day23,
                24 => MonthlyDeliveries.Day24,
                25 => MonthlyDeliveries.Day25,
                26 => MonthlyDeliveries.Day26,
                27 => MonthlyDeliveries.Day27,
                28 => MonthlyDeliveries.Day28,
                29 => MonthlyDeliveries.Day29,
                30 => MonthlyDeliveries.Day30,
                31 => MonthlyDeliveries.Day31,
                _ => throw new InvalidDateException()
            };
        }

        public class MonthlyDelivery
        {
            [ValidateComplexType]
            public required DailyDelivery Day1 { get; set; }
            public required DailyDelivery Day2 { get; set; }
            public required DailyDelivery Day3 { get; set; }
            public required DailyDelivery Day4 { get; set; }
            public required DailyDelivery Day5 { get; set; }
            public required DailyDelivery Day6 { get; set; }
            public required DailyDelivery Day7 { get; set; }
            public required DailyDelivery Day8 { get; set; }
            public required DailyDelivery Day9 { get; set; }
            public required DailyDelivery Day10 { get; set; }
            public required DailyDelivery Day11 { get; set; }
            public required DailyDelivery Day12 { get; set; }
            public required DailyDelivery Day13 { get; set; }
            public required DailyDelivery Day14 { get; set; }
            public required DailyDelivery Day15 { get; set; }
            public required DailyDelivery Day16 { get; set; }
            public required DailyDelivery Day17 { get; set; }
            public required DailyDelivery Day18 { get; set; }
            public required DailyDelivery Day19 { get; set; }
            public required DailyDelivery Day20 { get; set; }
            public required DailyDelivery Day21 { get; set; }
            public required DailyDelivery Day22 { get; set; }
            public required DailyDelivery Day23 { get; set; }
            public required DailyDelivery Day24 { get; set; }
            public required DailyDelivery Day25 { get; set; }
            public required DailyDelivery Day26 { get; set; }
            public required DailyDelivery Day27 { get; set; }
            public required DailyDelivery Day28 { get; set; }
            public required DailyDelivery Day29 { get; set; }
            public required DailyDelivery Day30 { get; set; }
            public required DailyDelivery Day31 { get; set; }
        }

        public class DailyDelivery
        {
            [DoubleType(ErrorMessage = "Number of Boxes is a decimal number")]
            public string? Price { get; set; }

            [IntType(min: 0)]
            public string? NumberOfBoxes { get; set; }
        }

        public class MonthOfTheYear
        {
            [Required]
            public Months Month { get; set; }

            [Required]
            public string? Year { get; set; }
        }
    }
}
