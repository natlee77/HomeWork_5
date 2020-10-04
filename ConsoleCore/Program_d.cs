
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Device
{
    class Program_d
    {
        // connection string till våran device i Azure IotHub 
        //Name in Device Explorer IoT_upp5
        private static readonly string _conn = "HostName=ecwin20IoTHub.azure-devices.net;DeviceId=IoT_upp5;SharedAccessKey=eRrd6k4uspN3/JBsiIVizyMgC8I/O1wGaj0isoWS0+E=";
        private static DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(_conn, TransportType.Mqtt);
        private static int telemetryInterval = 5;// 5 secund
        private static Random rnd = new Random();//  i async

        static void Main(string[] args)
        {
            deviceClient.SetMethodHandlerAsync("SetTelemetryInterval", SetTelemetryInterval, null).Wait();
            SendMessageAsync().GetAwaiter();

            Console.ReadKey();
        }

        // bygga 2st f.--static (hela programm)
        // 1 f.testar  set interval del
        //retunera <MethodResponse> //gör det med TASK
        private static Task<MethodResponse> SetTelemetryInterval(MethodRequest request, object userContext)
        //MethodRequest - request som styra med
        {
            var payload = Encoding.UTF8.GetString(request.Data).Replace("\"", "");

            if (Int32.TryParse(payload, out telemetryInterval))
            {

                Console.WriteLine(payload);
                Console.WriteLine($"Interval set to: {telemetryInterval} seconds.");

                // { "result": "Executed direct method: SetTelemetryInterval" }
                string json = "{\"result\": \"Executed direct method: " + request.Name + "\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 200));
            }
            else
            {
                string json = "{\"result\": \"Method not implemented\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(json), 501));
            }
        }


        // 2.f. tar testning 1 och skicka mdl
        private static async Task SendMessageAsync()
        {
            while (true)
            {
                double temp = 10 + rnd.NextDouble() * 15;
                double hum = 40 + rnd.NextDouble() * 20;

                var data = new
                {
                    temperature = temp,
                    humidity = hum
                };

                var json = JsonConvert.SerializeObject(data);
                var payload = new Message(Encoding.UTF8.GetBytes(json));
                payload.Properties.Add("temperatureAlert", (temp > 30) ? "true" : "false");

                await deviceClient.SendEventAsync(payload);
                Console.WriteLine($"Message sent: {json}");

                await Task.Delay(telemetryInterval * 1000);
            }


        }


    }
}   
