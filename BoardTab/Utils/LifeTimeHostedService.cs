using FluentScheduler;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BoardTab.Utils
{
    public class LifeTimeHostedService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Registry[] registries = new Registry[] { new InitialDBRegist() }; 
            JobManager.Initialize(registries); 
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            JobManager.RemoveAllJobs();
            JobManager.Stop();
            return Task.CompletedTask;
        }

        
    }
}
