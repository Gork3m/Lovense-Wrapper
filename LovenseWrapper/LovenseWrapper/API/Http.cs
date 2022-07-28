using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Net;
using System.Text;

namespace LovenseWrapper.API {
    public static class Http {
        public const string baseUrl = "https://c.lovense-api.com";
        public const string wsUrl = "wss://c.lovense-api.com";
        /// <summary>
        /// Gets the data from an access code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetIDInfo(string id) {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{baseUrl}/anon/controllink/join");
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            Stream memStream = new System.IO.MemoryStream();
            var end = Encoding.ASCII.GetBytes("\r\n--" + boundary);
            string body = "--" + boundary + "\r\nContent-Disposition: form-data; name=\"id\"\r\n\r\n" + id;

            byte[] bodybytes = Encoding.UTF8.GetBytes(body);
            memStream.Write(bodybytes, 0, bodybytes.Length);
            memStream.Write(end, 0, end.Length);
            request.ContentLength = memStream.Length;

            using (Stream requestStream = request.GetRequestStream()) {
                memStream.Position = 0;
                byte[] tempBuffer = new byte[memStream.Length];
                memStream.Read(tempBuffer, 0, tempBuffer.Length);
                memStream.Close();
                requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            }

            using (var response = request.GetResponse()) {
                Stream stream2 = response.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                return reader2.ReadToEnd();
            }
        }

    }
}
