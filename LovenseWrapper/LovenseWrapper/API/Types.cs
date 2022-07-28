using System;
using System.Collections.Generic;
using System.Text;

namespace LovenseWrapper.API {
    public class Types {

        public class WS {
            public class WSInitData {
                public string sid { get; set; }
                public List<string> upgrades { get; set; }
                public int pingInterval { get; set; }
                public int pingTimeout { get; set; }
                public bool wsClinetPingMust { get; set; }
                public int pingCode { get; set; }
                public int pongCode { get; set; }
            }

            public class CommandData {
                public class CommandPayload {
                    public List<object> array { get; set; }
                }

                public class ToyCommandJson {
                    public string toyCommandJson { get; set; }
                    public string linkId { get; set; }
                    public bool userTouch { get; set; }
                }

                public class VibrationData {
                    public int v { get; set; }
                    public int v1 { get; set; }
                    public int v2 { get; set; }
                    public int p { get; set; }
                    public int r { get; set; }
                }

                public class Id {
                    public VibrationData TOY_ID { get; set; }
                }

                public class ToyCommandJson_Value {
                    public string cate { get; set; }
                    public Id id { get; set; }
                }

            }

        }
        public class Http {
            public class ControlLinkData {
                public object ackId { get; set; }
                public string linkId { get; set; }
                public int version { get; set; }
                public string platform { get; set; }
                public int linkStatus { get; set; }
                public string selfId { get; set; }
                public Creator creator { get; set; }
                public Joiner joiner { get; set; }
                public Link link { get; set; }
                public string x { get; set; }
                public string y { get; set; }
                public bool joinerFirstTime { get; set; }
                public object punishment { get; set; }
            }

            public class Creator {
                public string userId { get; set; }
                public List<Toy> toys { get; set; }
                public bool newVersion { get; set; }
            }

            public class Data {
                public string wsUrl { get; set; }
                public string qrCode { get; set; }
                public bool fromCam { get; set; }
                public string socketIoPath { get; set; }
                public string idAlias { get; set; }
                public bool anotherPlaying { get; set; }
                public ControlLinkData controlLinkData { get; set; }
            }

            public class Joiner {
                public string userId { get; set; }
                public List<object> toys { get; set; }
                public object newVersion { get; set; }
            }

            public class Link {
                public object linkDesc { get; set; }
                public object tags { get; set; }
                public int expires { get; set; }
                public int leftControlTime { get; set; }
                public int startTimerTime { get; set; }
                public int expireDate { get; set; }
                public long createTime { get; set; }
                public bool isStart { get; set; }
                public object msgType { get; set; }
            }

            public class SessionInfo {
                public bool result { get; set; }
                public int code { get; set; }
                public object message { get; set; }
                public Data data { get; set; }
            }

            public class Toy {
                public string type { get; set; }
                public object isControl { get; set; }
                public string version { get; set; }
                public string name { get; set; }
                public string status { get; set; }
                public object battery { get; set; }
                public string id { get; set; }
                public string toyFun { get; set; }
            }
        }
        
    }
}
