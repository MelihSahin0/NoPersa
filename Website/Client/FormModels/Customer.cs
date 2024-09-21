using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using Website.Client.Exceptions;
using Website.Client.Util;

namespace Website.Client.FormModels
{
    public class Customer
    {
        public int Id { get; set; } = -1;

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

        [GeoCoordinatesType]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? GeoLocation { get; set; }

        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public string? ContactInformation { get; set; }

        [ValidateComplexType]
        [Required(ErrorMessage = "Month is required.")]
        public required MonthOfTheYear Month { get; set; }

        [Required]
        public required string Article { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public required string Price { get; set; }

        [Required(ErrorMessage = "Number of Boxes is required")]
        [IntType(min: 0)]
        public required string DefaultNumberOfBoxes { get; set; }

        [ValidateComplexType]
        [Required(ErrorMessage = "Monthly Delivery is required")]
        public required List<MonthlyDelivery> MonthlyDeliveries { get; set; }

        [Required]
        public Weekdays? Workdays { get; set; }

        [Required]
        public Weekdays? Holidays { get; set; }

        public DailyDelivery GetDailyDeliveryByDay(int day)
        {
            return day switch
            {
                1 => MonthlyDeliveries.First().Day1,
                2 => MonthlyDeliveries.First().Day2,
                3 => MonthlyDeliveries.First().Day3,
                4 => MonthlyDeliveries.First().Day4,
                5 => MonthlyDeliveries.First().Day5,
                6 => MonthlyDeliveries.First().Day6,
                7 => MonthlyDeliveries.First().Day7,
                8 => MonthlyDeliveries.First().Day8,
                9 => MonthlyDeliveries.First().Day9,
                10 => MonthlyDeliveries.First().Day10,
                11 => MonthlyDeliveries.First().Day11,
                12 => MonthlyDeliveries.First().Day12,
                13 => MonthlyDeliveries.First().Day13,
                14 => MonthlyDeliveries.First().Day14,
                15 => MonthlyDeliveries.First().Day15,
                16 => MonthlyDeliveries.First().Day16,
                17 => MonthlyDeliveries.First().Day17,
                18 => MonthlyDeliveries.First().Day18,
                19 => MonthlyDeliveries.First().Day19,
                20 => MonthlyDeliveries.First().Day20,
                21 => MonthlyDeliveries.First().Day21,
                22 => MonthlyDeliveries.First().Day22,
                23 => MonthlyDeliveries.First().Day23,
                24 => MonthlyDeliveries.First().Day24,
                25 => MonthlyDeliveries.First().Day25,
                26 => MonthlyDeliveries.First().Day26,
                27 => MonthlyDeliveries.First().Day27,
                28 => MonthlyDeliveries.First().Day28,
                29 => MonthlyDeliveries.First().Day29,
                30 => MonthlyDeliveries.First().Day30,
                31 => MonthlyDeliveries.First().Day31,
                _ => throw new InvalidDateException()
            };
        }

        public class MonthlyDelivery
        {
            public required MonthOfTheYear MonthOfTheYear { get; init; }

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
            [DoubleType(min: 0)]
            public string? Price { get; set; }

            [IntType(min: 0)]
            public string? NumberOfBoxes { get; set; }
        }

        public class MonthOfTheYear
        {
            [Required]
            public required Months Month { get; set; }

            [Required]
            public required string Year { get; set; }
        }
    }
}
