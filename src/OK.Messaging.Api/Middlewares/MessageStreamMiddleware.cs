using Microsoft.AspNetCore.Http;
using OK.Messaging.Core.Handlers;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OK.Messaging.Api.Middlewares
{
    public class MessageStreamMiddleware
    {
        private readonly IMessageStreamHandler _messageStreamHandler;
        private readonly RequestDelegate _next;

        public MessageStreamMiddleware(IMessageStreamHandler messageStreamHandler, RequestDelegate next)
        {
            _messageStreamHandler = messageStreamHandler;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                string identifier = context.User?.Identity.Name;

                if (string.IsNullOrEmpty(identifier))
                {
                    context.Response.StatusCode = 401;
                }

                WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();

                _messageStreamHandler.AddSocket(identifier, socket);

                await ListenConnection(identifier, socket);
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }

        private async Task ListenConnection(string identifier, WebSocket connection)
        {
            var buffer = new byte[1024];

            while (connection.State == WebSocketState.Open)
            {
                var result = await connection.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    if (string.IsNullOrEmpty(message))
                    {
                        return;
                    }

                    await _messageStreamHandler.ReceiveAsync(identifier, message);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await OnDisconnected(identifier, connection);
                }
            }
        }

        private async Task OnDisconnected(string identifier, WebSocket connection)
        {
            if (connection != null)
            {
                _messageStreamHandler.RemoveSocket(identifier, connection);

                await connection.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
            }
        }
    }
}