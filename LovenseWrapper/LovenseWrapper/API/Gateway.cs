using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Web;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace LovenseWrapper.API {
    public class Gateway {
        bool Connected;
        bool IsTokenInvalid = false;
        bool IsSocketAllowed = true;
        int heartbeatDelay = 1000;
        ClientWebSocket Socket;
        public Types.Http.SessionInfo SessionInfo { get; set; }
        Types.WS.WSInitData wsData { get; set; }

        public Gateway(Types.Http.SessionInfo sessionInfo) {
            Connected = false;
            SessionInfo = sessionInfo;
        }
        public void EndSession() {
            Disconnect();
            Connected = false;
        }
        public bool StartSession() {
            if (Connected) {
                Disconnect();
                Thread.Sleep(1000);
            }
            IsSocketAllowed = true;
            Connect();
            while (!Connected && !IsTokenInvalid) {
                Thread.Sleep(100);
            }
            return !IsTokenInvalid;
        }
        void Disconnect() {
            IsSocketAllowed = false;
            try {
                Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "IntentionalDisconnection", CancellationToken.None);
            } catch { }
        }
        public void Test() {

            try {
                Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "a", CancellationToken.None);
            } catch { }
        }

        async Task Connect() {
            if (!IsSocketAllowed || IsTokenInvalid) return;
            //IsSocketAllowed = true;
            bool isActiveSocket = true;
            Socket = new ClientWebSocket();
            Connected = false;
            try {
                Console.WriteLine(SessionInfo.data.wsUrl);

                await Socket.ConnectAsync(new Uri(SessionInfo.data.wsUrl.Replace("https://","wss://").Replace(".com?",".com/anon.io/?") + "&EIO=3&transport=websocket"), CancellationToken.None);
            } catch (Exception ex) {
                Console.WriteLine("Gateway Error: " + ex.Message); return;
            }


            try {
                ArraySegment<Byte> bfr = new ArraySegment<byte>(new Byte[4096]);
                WebSocketReceiveResult result = null;

                while (IsSocketAllowed && isActiveSocket) {
                    bool isEmpty = false;
                    using (var ms = new MemoryStream()) {
                        do {
                            try {
                                result = Socket.ReceiveAsync(bfr, CancellationToken.None).Result;
                                ms.Write(bfr.Array, bfr.Offset, result.Count);
                            } catch { isEmpty = true; break; }
                        }
                        while (!result.EndOfMessage);
                        if (isEmpty) continue;
                        ms.Seek(0, SeekOrigin.Begin);
                        if (result.MessageType == WebSocketMessageType.Close) {
                            using (var reader = new StreamReader(ms, Encoding.UTF8)) {
                                string reason = reader.ReadToEnd();
                                if (!reason.Contains("IntentionalDisconnection")) {
                                    ms.Dispose();
                                    if (result.CloseStatusDescription.Contains("Authentication failed")) {
                                        IsTokenInvalid = true;
                                        IsSocketAllowed = false;
                                        return;
                                    }
                                    isActiveSocket = false;
                                    if (IsSocketAllowed) {
                                        Console.WriteLine("Received Socket.Close");
                                        await Task.Delay(2000);
                                        Console.WriteLine("Reconnecting..");
                                        Connect();
                                        return;
                                    }
                                }
                            }
                        }

                        try {

                            if (result.MessageType == WebSocketMessageType.Text) {

                                using (var reader = new StreamReader(ms, Encoding.UTF8)) {
                                    string packet = reader.ReadToEnd();
                                    if (packet.StartsWith("0")) { // opcode 0: init payload 
                                        wsData = JsonConvert.DeserializeObject<Types.WS.WSInitData>(packet.Substring(1));
                                    }
                                    if (packet.StartsWith("40")) { // opcode 40: init completed - start heartbeating (maybe)
                                        Task.Factory.StartNew(async () => {
                                            Console.WriteLine("Starting heartbeat task...");
                                            while (isActiveSocket && IsSocketAllowed) {                                                
                                                await SendString(wsData.pingCode.ToString());
                                                Thread.Sleep(wsData.pingInterval);
                                            }
                                        });
                                    }
                                    if (packet.StartsWith(wsData.pongCode.ToString())) {
                                        // pong I'll handle this later
                                        Connected = true;
                                        SendString(@"42[""anon_open_control_panel_ts"",{""linkId"":"""+SessionInfo.data.controlLinkData.linkId+@"""}]");
                                        
                                    }
                                    

                                    
                                }
                            }
                        } catch (Exception ex) { Console.WriteLine("Exception while reading gateway packet: " + ex.Message, ConsoleColor.DarkRed); }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("Main Gateway Error: " + ex.Message, ConsoleColor.Red);

            }

        }



        public async Task SendString(String data) {
            var encoded = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<Byte>(encoded, 0, encoded.Length);
            await Socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
