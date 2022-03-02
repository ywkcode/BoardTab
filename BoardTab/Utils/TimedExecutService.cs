using BoardTab.Common;
using BoardTab.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BoardTab.Utils
{
    public class TimedExecutService : BackgroundService
    {
        public ILogger<TimedExecutService> _logger; 
        public TimedExecutService(ILogger<TimedExecutService> logger)
        {
            this._logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation(DateTime.Now.ToString() + "启动自动排障定时任务！");

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(5000, stoppingToken); //启动后五秒执行一次
                                                           //_logger.LogInformation(DateTime.Now.ToString() + " 执行自动排障任务！");

                    if (DateTime.Now.Hour == 5 && DateTime.Now.Minute == 0)
                    {
                        LogHelper.WriteLogs("执行定时任务！");
                        using (var scope = ConfigurationCache.RootServiceProvider.CreateScope())
                        {
                            IBoardService service = scope.ServiceProvider.GetService<IBoardService>();
                            service.InitailData();

                        }
                    }

                }
                _logger.LogInformation(DateTime.Now.ToString() + "自动排障任务停止！");
            }
            catch (Exception ex)
            {
                if (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation(DateTime.Now.ToString() + "自动排障任务异常！" + ex.Message + ex.StackTrace);
                }
                else
                {
                    _logger.LogInformation(DateTime.Now.ToString() + "自动排障任务异常停止！");
                }
            }

        }

    }
}
