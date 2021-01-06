using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketWebServiceExample
{
    public abstract class WebSocketHandler
    {
        protected ConnectionManage _connectionManage { get; set; }
        public WebSocketHandler(ConnectionManage connectionManage)
        {
            _connectionManage= connectionManage  ;
        }
        public virtual async Task OnConnected(WebSocket websocket)
        {
            _connectionManage.AddSocket(websocket);
        }
        public virtual async Task OnDisconnected(WebSocket websocket)
        {
            _connectionManage.RemoveSocket(_connectionManage.GetId(websocket));
        }
        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;
            await socket.SendAsync(new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                offset: 0,
                                                                count: message.Length), WebSocketMessageType.Text, endOfMessage: true, cancellationToken: CancellationToken.None);
        }
        public async Task SendMessageAsync(string socketId, string message)
        {
            await SendMessageAsync(_connectionManage.GetSocketById(socketId), message);
        }
        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in _connectionManage.GetAlls())
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendMessageAsync(pair.Value, message);
            }
        }
        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
