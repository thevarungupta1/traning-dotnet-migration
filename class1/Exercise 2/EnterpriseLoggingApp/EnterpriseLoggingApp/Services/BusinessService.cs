using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLoggingApp.Services
{
    public class BusinessService : IBusinessService
    {
        private readonly ILogger<BusinessService> _logger;

        public BusinessService(ILogger<BusinessService> logger)
        {
            _logger = logger;
        }
        public async Task ExecuteAsync()
        {
            _logger.LogInformation("Business operation started");

            try
            {
                await ProcessDataAsync();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error Occured while processing data");
            }

            _logger.LogInformation("Business operation completed successfully");
        }

        private async Task ProcessDataAsync()
        {
            _logger.LogDebug("Simulating data processing");

            await Task.Delay(1000);

            if (DateTime.Now.Second % 2 == 0)
            {
                throw new InvalidOperationException("Simulating failure condition");
            }
            _logger.LogInformation("Data processed successfully");
        }
    }
}
