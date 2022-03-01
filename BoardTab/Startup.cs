using BoardTab.Common;
using BoardTab.Services;
using BoardTab.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoardTab
{
    public class Startup
    {

        static Socket serverSocket;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<IBoardService, BoardService>();
            services.AddSingleton<IHostedService, TimedExecutService>();
            //services.AddSingleton<IHostedService, LifeTimeHostedService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app
            , IWebHostEnvironment env
            , IServiceProvider serviceProvider
            , IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            ConfigurationCache.RootServiceProvider = app.ApplicationServices;

            using (var serviceScope = serviceProvider.CreateScope())
            {
                IBoardService service = serviceScope.ServiceProvider.GetService<IBoardService>();

                service.InitailData();
            }

           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Board}/{action=Index}/{id?}");
            });


            #region 监听TCP

            lifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(() =>
                {
                    Console.WriteLine("启动一个Socket服务端");

                    int port = Convert.ToInt32(Configuration.GetSection("SocketSetting:Port").Value);
                    string host = Configuration.GetSection("SocketSetting:IP").Value ;//服务器端ip地址

                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint ipe = new IPEndPoint(ip, port);

                    serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    serverSocket.Bind(ipe);
                    serverSocket.Listen(10);
                    Console.WriteLine("等待客户端连接...");
                    LogHelper.WriteLogs("等待客户端连接...");
                    try
                    {
                        ThreadPool.QueueUserWorkItem(state => ListenClientSocket());
                        //Thread socketThread = new Thread(ListenClientSocket);
                        //socketThread.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Socket Error:" + ex.Message);
                    }
                    finally
                    {
                       // serverSocket.Close(); 
                    }
                });
            });
            #endregion
        }


        static void ListenClientSocket()
        {
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                Console.WriteLine("连接已建立....");
                #region 消息回发
                byte[] sendByte = Encoding.ASCII.GetBytes("success!");
                clientSocket.Send(sendByte, sendByte.Length, 0);
                #endregion

                #region 连接客户端，解析数据,多线程
                ThreadPool.QueueUserWorkItem(state => ReceiveSocket(clientSocket));
                //Thread receivethread = new Thread(ReceiveSocket); //委托方法
                //receivethread.Start(clientSocket);
                #endregion

            }
        }


        static void ReceiveSocket(object clientsocket)
        {
            Socket myclientSocket = (Socket)clientsocket;
            while (true)
            {
                string recStr = "";
                byte[] recBytes = new byte[4096];
                int bytes = myclientSocket.Receive(recBytes, recBytes.Length, 0);

                recStr += Encoding.ASCII.GetString(recBytes, 0, bytes);
                string RetMsg = $"客户端:{myclientSocket.RemoteEndPoint.ToString()},消息：{recStr},时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                //LogHelper.WriteLogs($"获得客户端消息：{RetMsg}");
                if (recStr.Equals("ADD"))
                {
                    using (var scope = ConfigurationCache.RootServiceProvider.CreateScope())
                    {
                        IBoardService service = scope.ServiceProvider.GetService<IBoardService>();
                        service.AddCurrentNum();
                    }
                }
            }
        }
    }
}
