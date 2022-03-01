using BoardTab.Services;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;

namespace BoardTab.Utils
{
    public class InitialDBRegist : Registry
    {
        public InitialDBRegist( )
        {
            NonReentrantAsDefault();

            Schedule<BoardUpdateJob>().ToRunEvery(1).Days().At(5, 02);  
        }
    }
    internal class BoardUpdateJob : IJob
    {
        public void Execute()
        {
            using (var scope = ConfigurationCache.RootServiceProvider.CreateScope())
            {
                IBoardService service = scope.ServiceProvider.GetService<IBoardService>();
                service.InitailData();

            }
        }
    }
}
