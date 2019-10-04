namespace Instaq.API.Extern.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using Instaq.Contract.Storage;
    using Microsoft.Extensions.Diagnostics.HealthChecks;

    public class HealthService : IHealthCheck
    {
        private IDebugStorage debugStorage;

        public HealthService(IDebugStorage debugStorage)
        {
            this.debugStorage = debugStorage;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                debugStorage.GetPhotosCount();
                return Task.FromResult(HealthCheckResult.Healthy("DB fine :')'"));
            }
            catch
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Something went wrong :'('"));
            }

        }

    }
}
