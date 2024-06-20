using Microsoft.Extensions.Hosting;
using OrderGeneratorAPI.ClientApp;
using OrderGeneratorAPI.Manager;
using QuickFix;
using QuickFix.Transport;
using System;

namespace OrderGeneratorAPI
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IOrderProcessManager, OrderProcessManager>();
            builder.Services.AddScoped<IOrderResponseManager, OrderResponseManager>();
            builder.Services.AddTransient<OrderGeneratorClientApp>();
            builder.Services.AddSignalR();

            var settings = new SessionSettings("./ordergenerator.cfg");
            var application = new OrderGeneratorClientApp(null);
            IMessageStoreFactory storeFactory = new FileStoreFactory(settings);
            ILogFactory logFactory = new ScreenLogFactory(settings);
            var socketInitiator = new SocketInitiator(application, storeFactory, settings, logFactory);
            socketInitiator.Start();

            var app = builder.Build();

            app.UseWebSockets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<ChatHub>("/chatHub/notifications");

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            app.Run();
        }
    }
}
        
