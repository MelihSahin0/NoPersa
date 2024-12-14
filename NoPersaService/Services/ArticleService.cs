using SharedLibrary.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Util;
using NoPersaService.Database;

namespace NoPersaService.Services
{
    public class ArticleService : IHostedService, IDisposable
    {
        private Timer? timer;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<ArticleService> logger;
      
        public ArticleService(IServiceProvider serviceProvider, ILogger<ArticleService> logger)
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

        public async Task CatchUp(NoPersaDbContext context, Maintenance maintenance)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                foreach (Article article in context.Articles)
                {
                    article.Name = article.NewName;
                    article.Price = article.NewPrice;
                }
                context.Maintenances.Remove(maintenance);
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

                    var maintenance = await context.Maintenances.FirstOrDefaultAsync(m => m.Type == MaintenanceTypes.Article.ToString());
                    if (maintenance != null)
                    {
                        if (DateTime.Today.Date >= maintenance.Date)
                        {
                            await CatchUp(context, maintenance);
                        }
                    }
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
