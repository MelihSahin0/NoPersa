namespace OpenXml.Models
{
    public class ExcelCustomer
    {
        public static readonly List<string> ExcelCustomerHeaderWidth = ["min-w-40", "min-w-40", "min-w-40", "min-w-40", "min-w-40", "min-w-40", "min-w-40", "min-w-40", "min-w-40", "min-w-40", "min-w-28", "min-w-28",
                                                                        "min-w-28", "min-w-28", "min-w-28", "min-w-28", "min-w-28", "min-w-28", "min-w-28",
                                                                        "min-w-28", "min-w-28", "min-w-28", "min-w-28", "min-w-28", "min-w-28", "min-w-28", "min-w-40"];
        public static List<string> ExcelCustomerHeader => ["Route", "Serial Number", "Title", "Name", "Address", "Region", "Latitude", "Longitude", "Delivery Whishes", "Contact Information", "Article", "Default Number of Boxes", 
                                                           "Workday - Monday", "Workday - Tuesday", "Workday - Wednesday", "Workday - Thursday", "Workday - Friday", "Workday - Saturday", "Workday - Sunday",
                                                           "Holiday - Monday", "Holiday - Tuesday", "Holiday - Wednesday", "Holiday - Thursday", "Holiday - Friday", "Holiday - Saturday", "Holiday - Sunday"];
        public static List<string> DefaultNumberOfBoxesSelection => ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10"];

        public required string RouteName { get; set; }
        public string? SerialNumber {  get; set; }
        public string? Title { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string Region { get; set; }
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
        public string? DeliveryWhishes { get; set; }
        public string? ContactInformation { get; set; }
        public required string Article { get; set; }
        public required int DefaultNumberOfBoxes { get; set; }
        public required bool WMonday { get; set; }
        public required bool WTuesday { get; set; }
        public required bool WWednesday { get; set; }
        public required bool WThursday { get; set; }
        public required bool WFriday { get; set; }
        public required bool WSaturday { get; set; }
        public required bool WSunday { get; set; }
        public required bool HMonday { get; set; }
        public required bool HTuesday { get; set; }
        public required bool HWednesday { get; set; }
        public required bool HThursday { get; set; }
        public required bool HFriday { get; set; }
        public required bool HSaturday { get; set; }
        public required bool HSunday { get; set; }
        public required List<string> Menus { get; set; }
        public required List<bool> LightDiets { get; set; }

        public static List<Table> Parse(List<ExcelCustomer> customer, List<string> header, List<string> routes, List<string> articles, int numberOfBoxContent, int numberOfLightDiets, List<string> portionSizes, List<string> trueAndFalse, bool splitToMultipleRoutes)
        {
            List<List<ExcelCustomer>> preparedCustomers = [];
            if (splitToMultipleRoutes)
            {
                var groupedByRoute = customer.GroupBy(m => m.RouteName).OrderBy(group => group.Key);
                if (groupedByRoute.Any())
                {
                    foreach (var group in groupedByRoute)
                    {
                        preparedCustomers.Add([.. group.OrderBy(g => g.Name)]);
                    }
                }
                else
                {
                    preparedCustomers.Add([.. customer.OrderBy(g => g.RouteName).ThenBy(g => g.Name)]);
                }
            }
            else
            {
                preparedCustomers.Add([.. customer.OrderBy(g => g.RouteName).ThenBy(g => g.Name)]);
            }

            List<Table.DropDownInformation> dopDownInformations = [
                        new() { Column = "A", Data = routes, StartRow = 2 },
                        new() { Column = "K", Data = articles, StartRow = 2 },
                        new() { Column = "L", Data = DefaultNumberOfBoxesSelection, StartRow = 2 },
                        new() { Column = "M", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "N", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "O", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "P", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "Q", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "R", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "S", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "T", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "U", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "V", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "W", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "X", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "Y", Data = trueAndFalse, StartRow = 2 },
                        new() { Column = "Z", Data = trueAndFalse, StartRow = 2 }
                    ];

            for (int i = 0; i < numberOfBoxContent; i++)
            {
                dopDownInformations.Add(new() { Column = SpreadSheetTable.GetExcelColumnName(i + 27), Data = portionSizes, StartRow = 2 });
            }

            for (int i = 0; i < numberOfLightDiets; i++)
            {
                dopDownInformations.Add(new() { Column = SpreadSheetTable.GetExcelColumnName(i + 27 + numberOfBoxContent), Data = trueAndFalse, StartRow = 2 });
            }

            var tables = new List<Table>();
            foreach (var preparedCustomer in preparedCustomers)
            {
                tables.Add(new Table
                {
                    Name = preparedCustomers.Count == 1 ? "All_Routes" : Table.Sanitize(preparedCustomer[0].RouteName),
                    Headers = header,
                    Data = preparedCustomer.Select(customer =>
                    {
                        var row = new List<object?>
                            {
                                customer.RouteName,
                                customer.SerialNumber,
                                customer.Title,
                                customer.Name,
                                customer.Address,
                                customer.Region,
                                customer.Latitude,
                                customer.Longitude,
                                customer.DeliveryWhishes,
                                customer.ContactInformation,
                                customer.Article,
                                customer.DefaultNumberOfBoxes,
                                customer.WMonday,
                                customer.WTuesday,
                                customer.WWednesday,
                                customer.WThursday,
                                customer.WFriday,
                                customer.WSaturday,
                                customer.WSunday,
                                customer.HMonday,
                                customer.HTuesday,
                                customer.HWednesday,
                                customer.HTuesday,
                                customer.HFriday,
                                customer.HSaturday,
                                customer.HSaturday,
                        };
                        row.AddRange(customer.Menus.Select(portion => (object?)portion));
                        row.AddRange(customer.LightDiets.Concat(Enumerable.Repeat(false, numberOfLightDiets - customer.LightDiets.Count))
                                                        .Select(lightDiet => (object?)lightDiet));

                        return row;
                    }).ToList(),
                    DropDownInformations = dopDownInformations
                });
            }

            return tables;
        }

        public static List<List<ExcelCustomer>> Parse(List<Table> tables)
        {
            List<List<ExcelCustomer>> complexExcelCustomers = [];

            foreach (Table table in tables)
            {
                List<ExcelCustomer> excelCustomers = [];
                List<List<object?>> data = table.Data;
                for (int i = 0; i < data.Count; i++)
                {
                    var customer = new ExcelCustomer
                    {
                        RouteName = string.Empty,
                        SerialNumber = string.Empty,
                        Title = string.Empty,
                        Name = string.Empty,
                        Address = string.Empty,
                        Region = string.Empty,
                        Latitude = 0.0,
                        Longitude = 0.0,
                        DeliveryWhishes = string.Empty,
                        ContactInformation = string.Empty,
                        Article = string.Empty,
                        DefaultNumberOfBoxes = 1,
                        WMonday = false,
                        WTuesday = false,
                        WWednesday = false,
                        WThursday = false,
                        WFriday = false,
                        WSaturday = false,
                        WSunday = false,
                        HMonday = false,
                        HTuesday = false,
                        HWednesday = false,
                        HThursday = false,
                        HFriday = false,
                        HSaturday = false,
                        HSunday = false,
                        Menus = [],
                        LightDiets = [],
                    };

                    for (int j = 0; j < data[i].Count; j++)
                    {
                        var value = data[i][j];

                        switch (j)
                        {
                            case 0:
                                customer.RouteName = Convert.ToString(value) ?? string.Empty;
                                break;
                            case 1:
                                customer.SerialNumber = Convert.ToString(value) ?? string.Empty;
                                break;
                            case 2:
                                customer.Title = Convert.ToString(value) ?? string.Empty;
                                break;
                            case 3:
                                customer.Name = Convert.ToString(value) ?? string.Empty;
                                break;
                            case 4:
                                customer.Address = Convert.ToString(value) ?? string.Empty;
                                break;
                            case 5:
                                customer.Region = Convert.ToString(value) ?? string.Empty;
                                break;
                            case 6:
                                customer.Latitude = Convert.ToDouble(value);
                                break;
                            case 7:
                                customer.Longitude = Convert.ToDouble(value);
                                break;
                            case 8:
                                customer.DeliveryWhishes = Convert.ToString(value) ?? string.Empty;
                                break;
                            case 9:
                                customer.ContactInformation = Convert.ToString(value) ?? string.Empty;
                                break;
                            case 10:
                                customer.Article = Convert.ToString(value) ?? string.Empty;
                                break;
                            case 11:
                                customer.DefaultNumberOfBoxes = Convert.ToInt32(value);
                                break;
                            case 12:
                                customer.WMonday = Convert.ToBoolean(value);
                                break;
                            case 13:
                                customer.WTuesday = Convert.ToBoolean(value);
                                break;
                            case 14:
                                customer.WWednesday = Convert.ToBoolean(value);
                                break;
                            case 15:
                                customer.WThursday = Convert.ToBoolean(value);
                                break;
                            case 16:
                                customer.WFriday = Convert.ToBoolean(value);
                                break;
                            case 17:
                                customer.WSaturday = Convert.ToBoolean(value);
                                break;
                            case 18:
                                customer.WSunday = Convert.ToBoolean(value);
                                break;
                            case 19:
                                customer.HMonday = Convert.ToBoolean(value);
                                break;
                            case 20:
                                customer.HTuesday = Convert.ToBoolean(value);
                                break;
                            case 21:
                                customer.HWednesday = Convert.ToBoolean(value);
                                break;
                            case 22:
                                customer.HThursday = Convert.ToBoolean(value);
                                break;
                            case 23:
                                customer.HFriday = Convert.ToBoolean(value);
                                break;
                            case 24:
                                customer.HSaturday = Convert.ToBoolean(value);
                                break;
                            case 25:
                                customer.HSunday = Convert.ToBoolean(value);
                                break;
                            default:
                                if (value is bool)
                                {
                                    customer.LightDiets.Add(Convert.ToBoolean(value));
                                }
                                else
                                {
                                    customer.Menus.Add(Convert.ToString(value) ?? string.Empty);
                                }
                                break;
                        }
                    }

                    excelCustomers.Add(customer);
                }

                complexExcelCustomers.Add(excelCustomers);
            }

            return complexExcelCustomers;
        }
    }
}
