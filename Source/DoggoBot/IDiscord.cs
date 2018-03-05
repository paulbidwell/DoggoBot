using System.Threading.Tasks;
using Discord.WebSocket;

namespace DoggoBot
{
    public interface IDiscord
    {
        Task MessageReceived(SocketMessage message);
    }
}
