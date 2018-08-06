using OK.Messaging.Core.Handlers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OK.Messaging.Engine.Handlers
{
    public class MessageStreamHandler : IMessageStreamHandler
    {
        private ConcurrentDictionary<string, List<WebSocket>> userConnections =
               new ConcurrentDictionary<string, List<WebSocket>>();

        public List<WebSocket> GetSockets(string identifier)
        {
            List<WebSocket> connections;

            userConnections.TryGetValue(identifier, out connections);

            return connections ?? new List<WebSocket>();
        }

        public void AddSocket(string identifier, WebSocket socket)
        {
            List<WebSocket> connections = userConnections.GetOrAdd(identifier, new List<WebSocket>());

            connections.Add(socket);
        }

        public void RemoveSocket(string identifier, WebSocket socket)
        {
            List<WebSocket> connections;

            userConnections.TryGetValue(identifier, out connections);

            if (connections != null)
            {
                connections.RemoveAll(x => x == socket);

                if (!connections.Any())
                {
                    userConnections.TryRemove(identifier, out _);
                }
            }
        }

        public async Task SendAsync(string identifier, string message)
        {
            List<WebSocket> connections = GetSockets(identifier);

            foreach (var socket in connections)
            {
                if (socket == null)
                    return;

                if (socket.State != WebSocketState.Open)
                {
                    RemoveSocket(identifier, socket);

                    return;
                }

                var arr = Encoding.UTF8.GetBytes(message);

                var buffer = new ArraySegment<byte>(arr, 0, arr.Length);

                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task ReceiveAsync(string identifier, string message)
        {
            throw new NotImplementedException();
        }
    }
}