namespace NoPersaService.DTOs.ImportExport.Receive
{
    public class DTOInvoice
    {
        public int Year { get; set; }

        public int Month { get; set; }
        
        public bool SplitToMultipleRoutes { get; set; }
     
        public bool ShowAllDays { get; set; }
    }
}
