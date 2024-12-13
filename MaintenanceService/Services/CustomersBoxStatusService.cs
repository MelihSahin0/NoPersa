using MaintenanceService.Database;
using SharedLibrary.Models;

namespace MaintenanceService.Services
{
    public class CustomersBoxStatusService : IHostedService, IDisposable
    {
        private Timer? timer;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<CustomersBoxStatusService> logger;
      
        public CustomersBoxStatusService(IServiceProvider serviceProvider, ILogger<CustomersBoxStatusService> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var targetTime = DateTime.Today.AddHours(02);

            var initialDelay = targetTime - now;
            if (initialDelay < TimeSpan.Zero)
            {
                initialDelay = TimeSpan.Zero;
            }

            timer = new Timer(DoWork, null, initialDelay, TimeSpan.FromHours(24));

            return Task.CompletedTask;
        }

        public async Task CatchUp(NoPersaDbContext context)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                foreach (BoxStatus boxStatus in context.BoxStatus)
                {
                    boxStatus.NumberOfBoxesPreviousDay = boxStatus.NumberOfBoxesCurrentDay;
                    boxStatus.ReceivedBoxes = 0;
                    boxStatus.DeliveredBoxes = 0;
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(ex, "An error occurred while processing maintenance and updating articles.");
            }
        }

        private void DoWork(object? state)
        {
            Task.Run(async () =>
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<NoPersaDbContext>();
                    await CatchUp(context);
                }
            }).ConfigureAwait(true);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
