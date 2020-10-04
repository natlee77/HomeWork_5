using Microsoft.Azure.Devices;
using System;
using System.Threading.Tasks;

namespace Service
{
    public class Program_s
    {
        private static ServiceClient serviceClient = ServiceClient.CreateFromConnectionString("HostName=Alex-win20-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=wCccy5w2hwSvtvRNQb5gSYGStf7Xm7+1gTkYs7oEz7M=");

        static void Main(string[] args)
        {
            Task.Delay(5000).Wait();

            InvokeMethod("DeviceApp", "SetTelemetryInterval", "10").GetAwaiter();
            Console.ReadKey();
        }

        static async Task InvokeMethod(string deviceId, string methodName, string payload)
        {
            var methodInvocation = new CloudToDeviceMethod(methodName) { ResponseTimeout = TimeSpan.FromSeconds(30) };
            methodInvocation.SetPayloadJson(payload);


            var response = await serviceClient.InvokeDeviceMethodAsync(deviceId, methodInvocation);
            Console.WriteLine($"Response Status: {response.Status}");
            Console.WriteLine(response.GetPayloadAsJson());
        }

        //public static CloudToDeviceMethod GetCloudToDeviceMethod(string methodName)
        //{
        //    return new CloudToDeviceMethod(methodName) { ResponseTimeout = TimeSpan.FromSeconds(30) };
        //}
    }

}
