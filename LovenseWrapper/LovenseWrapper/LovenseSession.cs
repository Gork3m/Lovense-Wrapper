using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using LovenseWrapper.API;
using LovenseWrapper.Debugging;


namespace LovenseWrapper {
    public class LovenseSession {
        private string code { get; set; }
        public LovenseSession(string access_code) {
            code = access_code;
        }
        public List<LovenseToy> toys = new List<LovenseToy>() { };
        public LovenseToy toy { get; set; } // primary toy
        Gateway gateway { get; set; }
        Types.Http.SessionInfo sessionInfo { get; set; }
        public string error { get; set; }
        public bool Connect() {
            string response = Http.GetIDInfo(code);
            Types.Http.SessionInfo t = JsonConvert.DeserializeObject<Types.Http.SessionInfo>(response);
            if (t.message != null) {
                Debug.Log("[WARNING] " + t.message, ConsoleColor.Red);
                error = t.message.ToString();
                return false;
            }
            Debug.Log("[INFO] Access code is valid!", ConsoleColor.Green);
            sessionInfo = t;
            gateway = new Gateway(t);
            if (!gateway.StartSession()) {
                error = "Unexpected gateway error";
                Debug.Log("[ERROR] Unable to connect to websocket", ConsoleColor.Red);
                return false;
            }
            
            foreach (Types.Http.Toy toy in t.data.controlLinkData.creator.toys) {
                toys.Add(new LovenseToy(toy, sessionInfo, gateway));
            }
            toy = toys[0];
            return true;
        }

        public void Disconnect() {
            gateway.EndSession();

        }

    }
}
