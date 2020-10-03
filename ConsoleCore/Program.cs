using LibraryCore.Services;
using Microsoft.Azure.Devices.Client;
using System;

namespace ConsoleCore
{
    class Program
    {
        // connection string till våran device i Azure IotHub 
        //Name in Device Explorer IoT_upp5
        private static readonly string _conn = "HostName=ecwin20IoTHub.azure-devices.net;DeviceId=IoT_upp5;SharedAccessKey=eRrd6k4uspN3/JBsiIVizyMgC8I/O1wGaj0isoWS0+E=";
        private static int telemetryInterval = 5;

        // behöver ha (""- connection string till IoT apparat / vi tar från Azure
        // Mqtt- kommunikation protokol mdl=4 kb/ http -stor 
        // koppla   Device  och IoTHub(molnet) -- 
        private static readonly DeviceClient deviceClient = DeviceClient.
       CreateFromConnectionString(_conn, TransportType.Mqtt);


        static void Main(string[] args)
        {

            DeviceService.SendMessageAsync(deviceClient).GetAwaiter();
            DeviceService.ReceiveMessageAsync(deviceClient).GetAwaiter();

            Console.ReadKey();// låsa ConsoleApp -jag kan se 

        }
    }
}
