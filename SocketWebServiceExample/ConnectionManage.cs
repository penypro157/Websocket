using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace SocketWebServiceExample
{
    public class ConnectionManage
    {
        private ConcurrentDictionary<string, WebSocket> _socket = new ConcurrentDictionary<string, WebSocket>();
        public WebSocket GetSocketById(string id)
        {
            return _socket.FirstOrDefault(p => p.Key == id).Value;
        }
        public ConcurrentDictionary<string, WebSocket> GetAlls()
        {
            return _socket;
        }
        public string GetId(WebSocket socket)
        {
            return _socket.FirstOrDefault(p => p.Value == socket).Key;
        }
        public void AddSocket(WebSocket socket)
        {
            _socket.TryAdd(CreateConnectionId(), socket);
        }
        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
        public async Task RemoveSocket(string id)
        {
            WebSocket socket;
            _socket.TryRemove(id, out socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection Closed !!!", cancellationToken: CancellationToken.None);
        }
    }
}
