using AppyNox.Services.License.SharedEvents.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppyNox.Services.Authentication.Infrastructure.MassTransit.Consumers
{
    public class TemporaryLicenseValidationCompletedConsumer : IConsumer<LicenseValidationCompleted>
    {
        #region Public Methods

        public Task Consume(ConsumeContext<LicenseValidationCompleted> context)
        {
            Console.WriteLine("test");
            return Task.CompletedTask;
        }

        #endregion
    }
}