using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LibraryCore.Models;
using MAD = Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace LibraryCore.Services
{
    
    public static class DeviceService
    {
        private static readonly Random rnd = new Random();

                                                           //DEVICE CLIENT= IOT DEVICE (BIL)
                                                          //INSTALERADE Microsoft.Azure.Devices.Client
        public static async Task SendMessageAsync(DeviceClient deviceClient)
        {
           
            while (true)
            {
                var data = new TemperatureModel()
                {
                    Temperature = rnd.Next(20, 30),
                    Humidity = rnd.Next(30, 50)
                };

                                          // mdl konvertera i json format{"temperature":20, "humidity": 44}
                var json = JsonConvert.SerializeObject(data);

                                           //skicka mdl=payload/ Message-från  Microsoft.Azure.Devices.Client;
                                              //Encoding-formatera-- packetera ( 0 eller 1)
                                              //  alias i using system  MAD -Microsoft.Azure.Devices
                var payload = new Message(Encoding.UTF8.GetBytes(json));
                await deviceClient.SendEventAsync(payload);//använda message async =  skicka mdl i molnet

                Console.WriteLine($"Message sent : {json}");
                await Task.Delay(60 * 1000);
            }
        }



                                               //  DEVICE CLIENT = IOT DEVICE
                                               // f. ska skicka mdl till enheten device 
                                               //  skapa egna async tråd från Main Programm
        public static async Task ReceiveMessageAsync(DeviceClient deviceClient)
        {
            while (true) 
            {
                var payload = await deviceClient.ReceiveAsync();

                if (payload == null)
                    continue;                       // försätter loopar igen 

                Console.WriteLine($"Message Received:{Encoding.UTF8.GetString(payload.GetBytes())}");
                // GetString hämta  text från (payload.GetBytes= 0;1 //formatera som UTF8

                await deviceClient.CompleteAsync(payload);        // ta bort mdl från Hub
            }
        }




        // SERVICE CLIENT = IOT HUB  -- SIMULERAR IOT HUB// SERVICE -UTFÖRA NÅNTNG(STYRA)
        // INSTALERA -SERVICE SDK- MAD -alias--Microsoft.Azure.Devices        
        public static async Task SendMessageToDeviceAsync(MAD.ServiceClient serviceClient, string targetDeviceId, string message)
        {
            var payload = new MAD.Message(Encoding.UTF8.GetBytes(message));
            await serviceClient.SendAsync(targetDeviceId, payload);

        }
        // +++ Azure Function för att köra det 
    }
}