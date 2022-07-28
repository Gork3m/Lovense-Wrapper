using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using LovenseWrapper.API;
using LovenseWrapper.Debugging;

namespace LovenseWrapper {
    public class LovenseToy {
        public LovenseToy(Types.Http.Toy _toyInfo, Types.Http.SessionInfo _sessionInfo, Gateway _gateway) {
            toyInfo = _toyInfo;
            sessionInfo = _sessionInfo;
            gateway = _gateway;
        }
        Types.Http.Toy toyInfo { get; set; }
        Types.Http.SessionInfo sessionInfo { get; set; }
        Gateway gateway { get;set; }

        public void Vibrate(int power) {
            string payload = JsonConvert.SerializeObject((new Types.WS.CommandData.CommandPayload() {
                array = new List<object>() {
                    "anon_command_link_ts",
                    new Types.WS.CommandData.ToyCommandJson() {
                        toyCommandJson = JsonConvert.SerializeObject(new Types.WS.CommandData.ToyCommandJson_Value() { 
                            cate = "id",
                            id = new Types.WS.CommandData.Id() {
                                TOY_ID = new Types.WS.CommandData.VibrationData {
                                    v = -1,
                                    v1 = power,
                                    v2 = power,
                                    p = -1,
                                    r = -1
                                }
                            }
                        }),
                        linkId = sessionInfo.data.controlLinkData.linkId,
                        userTouch = false
                    }
                }
            }).array);
           
            gateway.SendString("42" + payload.Replace("TOY_ID", toyInfo.id));
        }
    }
}
