using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AgerDevice.Services
{
    public class AcquisitionService : IHostedService
    {
        public AcquisitionService()
        {

        }

        public async Task StartAsync(CancellationToken cancellationToken) { }

        public async Task StopAsync(CancellationToken cancellationToken) { }
    }
}
