namespace OpenXml.Models
{
    public class Invoice
    {
        public static readonly List<string> InvoiceHeaderWidth = ["min-w-40", "min-w-40", "min-w-28", "min-w-28"];
        public static List<string> InvoiceHeader => ["Serial Number", "Name", "Total Sales"];

        public required string RouteName { get; set; }
        public string? SerialNumber { get; set; }
        public required string CustomerName { get; set; }
        public required double TotalSales { get; set; }
        public required List<double> DailySales { get; set; }

        public static List<Table> Parse(List<Invoice> invoices, List<string> header, bool splitToMultipleRoutes, bool showAllDays)
        {
            List<List<Invoice>> preparedMonthlyOverviews = [];
            if (splitToMultipleRoutes)
            {
                var groupedByRoute = invoices.GroupBy(m => m.RouteName).OrderBy(group => group.Key);
                if (groupedByRoute.Any())
                {
                    foreach (var group in groupedByRoute)
                    {
                        preparedMonthlyOverviews.Add([.. group.OrderBy(g => g.CustomerName)]);
                    }
                }
                else
                {
                    preparedMonthlyOverviews.Add([.. invoices.OrderBy(m => m.CustomerName)]);
                }
            }
            else
            {
                preparedMonthlyOverviews.Add([.. invoices.OrderBy(m => m.CustomerName)]);
            }

            var tables = new List<Table>();
            foreach (var preparedMonth in preparedMonthlyOverviews)
            {
                tables.Add(new Table
                {
                    Name = preparedMonthlyOverviews.Count == 1 ? "All_Routes" : Table.Sanitize(preparedMonth[0].RouteName),
                    Headers = header,
                    Data = preparedMonth.Select(invoice =>
                    {
                        var row = new List<object?>
                            {
                                    invoice.SerialNumber,
                                    invoice.CustomerName,
                                    invoice.TotalSales
                            };

                        if (showAllDays)
                        {
                            row.AddRange(invoice.DailySales.Select(portion => (object?)portion));
                        }

                        return row;
                    }).ToList()
                });
            }

            return tables;
        }

        public static List<List<Invoice>> Parse(List<Table> tables)
        {
            List<List<Invoice>> complexInvoices = [];

            foreach (Table table in tables)
            {
                List<Invoice> invoices = [];
                List<List<object?>> data = table.Data;
                for (int i = 0; i < data.Count; i++)
                {
                    var invoice = new Invoice()
                    {
                        SerialNumber = null,
                        CustomerName = string.Empty,
                        RouteName = table.Name,
                        TotalSales = 0,
                        DailySales = []
                    };

                    for (int j = 0; j < data[i].Count; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                invoice.SerialNumber = Convert.ToString(data[i][j]) ?? string.Empty;
                                break;
                            case 1:
                                invoice.CustomerName = Convert.ToString(data[i][j]) ?? string.Empty;
                                break;
                            case 2:
                                invoice.TotalSales = Convert.ToDouble(data[i][j]);
                                break;
                            default:
                                invoice.DailySales.Add(Convert.ToDouble(data[i][j]));
                                break;
                        }
                    }

                    invoices.Add(invoice);
                }

                complexInvoices.Add(invoices);
            }

            return complexInvoices;
        }
    }
}
