using LibraryCore.Services;
using Microsoft.Azure.Devices.Client;
using System;

namespace ConsoleCore
{
    class Program
    {
        // connection string till våran device i Azure IotHub
        private static readonly string _conn = "HostName=ecwin20IoTHub.azure-devices.net;DeviceId=consoleapp;SharedAccessKey=3RSw06VBsoW/NBcIcqQ2tSm6tgWUoNDD+GxRHARsZ78=";


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
