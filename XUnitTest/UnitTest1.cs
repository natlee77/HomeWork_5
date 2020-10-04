using Device.Services;
using Microsoft.Azure.Devices.Client;
using Prism.Services;
using System;
using Xunit;

namespace XUnitTest
{
    public class UnitTest 
    {

        private DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=ecwin20IoTHub.azure-devices.net;DeviceId=IoT_upp5;SharedAccessKey=eRrd6k4uspN3/JBsiIVizyMgC8I/O1wGaj0isoWS0+E=", TransportType.Mqtt);
        [Theory]
        [InlineData("SetTelemetryInterval", "10", 200)]
        [InlineData("SetInterval", "10", 501)]
        public void SetTelemetryInterval_ShouldChangeTheInterval(string methodName, string payload, int statusCode)
        {
            deviceClient.SetMethodHandlerAsync("SetTelemetryInterval", DeviceService.SetTelemetryInterval, null).Wait(); // anroppa 
        }
    }
}
