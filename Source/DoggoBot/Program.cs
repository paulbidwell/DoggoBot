using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DoggoBot.CognitiveServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DoggoBot
{
    public class Program
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            var serviceProvider = new ServiceCollection()     
                .AddSingleton(Configuration)
                .AddScoped<IComputerVision, ComputerVision>()
                .AddScoped<IDiscord, Discord>()
                .BuildServiceProvider();

            var discord = serviceProvider.GetService<IDiscord>();

            var client = new DiscordSocketClient();

            client.Log += Log;
            client.MessageReceived += discord.MessageReceived;

            var discordToken = Configuration["DiscordToken"];
            await client.LoginAsync(TokenType.Bot, discordToken);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}