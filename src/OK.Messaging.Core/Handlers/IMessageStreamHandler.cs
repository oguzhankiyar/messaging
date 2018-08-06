using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace OK.Messaging.Core.Handlers
{
    public interface IMessageStreamHandler
    {
        List<WebSocket> GetSockets(string identifier);

        void AddSocket(string identifier, WebSocket socket);

        void RemoveSocket(string identifier, WebSocket socket);

        Task SendAsync(string identifier, string message);

        Task ReceiveAsync(string identifier, string message);
    }
}