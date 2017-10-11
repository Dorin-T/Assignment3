using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace tcpListenerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 5000;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            var server = new TcpListener(localAddr, port);

            server.Start();

            Console.WriteLine("Started");


            while (true)
            {
                var client = server.AcceptTcpClient();

                Console.WriteLine("Client connected");

                var thread = new Thread(HandleClient);

                thread.Start(client);
            }
        }

        static void HandleClient(object clientObj)
        {
            //create list of categories...
            var categories = new List<object>
            {
                new {cid = 1, name = "Beverages"},
                new {cid = 2, name = "Condiments"},
                new {cid = 3, name = "Confections"}
            };
            //Or create Object which one is better choose later...
            Beverages addBeverages  = new Beverages();

            addBeverages.Id = 4;
            addBeverages.name = "beer";

            categories.Add(addBeverages);


            var client = clientObj as TcpClient;
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;
            if (client == null) return;

            var strm = client.GetStream();

            int i;

            // Loop to receive all the data sent by the client.
            while ((i = strm.Read(bytes, 0, bytes.Length)) != 0)
            {
                //response object
                Response response = new Response();
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                //json text to object
                RequestObj requestObj = JsonConvert.DeserializeObject<RequestObj>(data);

                if (requestObj.method.Equals("echo"))
                {
                    response.Status = "1 Ok";
                    response.Body = requestObj.body;

                }else if (requestObj.method.Equals("read") && requestObj.path.Equals("/api/categories"))
                {
                    //read dummy list
                    response.Status = "1 Ok";
                    var categoryAsJson = JsonConvert.SerializeObject(categories);
                    response.Body = categoryAsJson;
                }
                else if (requestObj.method.Equals("read") && requestObj.path.Equals("/api/categories/1"))
                {
                    //read dummy list
                    response.Status = "1 Ok";
                    //hardcoded bad but for now OK
                    var categoryAsJson = JsonConvert.SerializeObject(categories[0]);
                    response.Body = categoryAsJson;
                }
                else if (requestObj.method.Equals("read") && requestObj.path.Equals("/api/categories/1"))
                {
                    //read dummy list
                    response.Status = "1 Ok";
                    //hardcoded bad but for now OK
                    var categoryAsJson = JsonConvert.SerializeObject(categories[0]);
                    response.Body = categoryAsJson;
                }
                else if (requestObj.method.Equals("update") && requestObj.path.Equals("/api/categories/1"))
                {
                    //update dummy list
                    categories[0] = new {requestObj.body};
                    response.Status = "3 updated";
                    
                }
                else if (requestObj.method.Equals("update") && requestObj.path.Equals("/api/categories"))
                {
                    response.Status = "4 Bad Request";

                }
                else if (requestObj.method.Equals("create") && requestObj.path.Equals("/api/categories"))
                {

                    var newName = requestObj.body.Trim(new Char[] {'/', '\\'});

                    var newNameJsonObject2 = JsonConvert.DeserializeObject<Beverages>(newName);

                    Beverages createNameBody = new Beverages();
                    createNameBody.Id = 5;
                    createNameBody.name = newNameJsonObject2.name;

                    categories.Add(createNameBody);

                    response.Status = "2 Created";

                }
                else if (requestObj.method.Equals("delete") && requestObj.path.Equals("/api/categories/5"))
                {
                    //also hardcoded could be done better if needed
                    categories.RemoveAt(4);
                    response.Status = "1 Ok";
                }
                else if (requestObj.method.Equals("delete") && requestObj.path.Equals("/api/categories/1234"))
                {

                    response.Status = "5 not found";
                }
                else if (requestObj.method.Equals("read") && requestObj.path.Equals("/api/categories/123"))
                {

                    response.Status = "5 not found";
                }
                //Console.WriteLine(categories[0].ToString());

                var serverMessage = JsonConvert.SerializeObject(response);


                byte[] msg = System.Text.Encoding.ASCII.GetBytes(serverMessage);

                // Send back a response.
                strm.Write(msg, 0, msg.Length);
                Console.WriteLine("Sent: "+ response.Status+" "+response.Body);
            }

            strm.Close();

            client.Dispose();
        }

        public class Response
        {
            public string Status { get; set; }
            public string Body { get; set; }
        }

        public class Category
        {
            [JsonProperty("cid")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
        }
        public class RequestObj
        {
            [JsonProperty("method")]
            public string method { get; set; }
            [JsonProperty("path")]
            public string path { get; set; }
            [JsonProperty("date")]
            public Int32 date { get; set; }
            [JsonProperty("body")]
            public string body { get; set; }

        }

        public class Beverages
        {
            [JsonProperty("cid")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string name { get; set; }
        }





    }
}
