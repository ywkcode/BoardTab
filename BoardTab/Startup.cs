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


            #region ����TCP

            lifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(() =>
                {
                    Console.WriteLine("����һ��Socket�����");

                    int port = Convert.ToInt32(Configuration.GetSection("SocketSetting:Port").Value);
                    string host = Configuration.GetSection("SocketSetting:IP").Value ;//��������ip��ַ

                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint ipe = new IPEndPoint(ip, port);

                    serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    serverSocket.Bind(ipe);
                    serverSocket.Listen(10);
                    Console.WriteLine("�ȴ��ͻ�������...");
                    LogHelper.WriteLogs("�ȴ��ͻ�������...");
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
                Console.WriteLine("�����ѽ���....");
                #region ��Ϣ�ط�
                byte[] sendByte = Encoding.ASCII.GetBytes("success!");
                clientSocket.Send(sendByte, sendByte.Length, 0);
                #endregion

                #region ���ӿͻ��ˣ���������,���߳�
                ThreadPool.QueueUserWorkItem(state => ReceiveSocket(clientSocket));
                //Thread receivethread = new Thread(ReceiveSocket); //ί�з���
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
                string RetMsg = $"�ͻ���:{myclientSocket.RemoteEndPoint.ToString()},��Ϣ��{recStr},ʱ��:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                //LogHelper.WriteLogs($"��ÿͻ�����Ϣ��{RetMsg}");
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
