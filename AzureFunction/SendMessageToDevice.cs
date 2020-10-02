using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using LibraryCore.Models;
using LibraryCore.Services;
using Microsoft.Azure.Devices;

namespace AzureFunction
{
    public static class SendMessageToDevice
    {
        private static readonly ServiceClient serviceClient =
            ServiceClient.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHubConnection"));
        // access policy IoTHub -- tog från Azure Iothub och lägger i local.setting.json


        [FunctionName("SendMessageToDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //Query string= localhost.7071/api/sendmessagetodevice?targetdeviceid=consoleapp&message=dettarrmeddelandet
            string targetDeviceId = req.Query["targetdeviceid"];
            string message = req.Query["message"];

            //Http Body= {"targetdeviceid": "consoleapp", "message":"dett är meddelandet"}
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var data = JsonConvert.DeserializeObject<MessageModel>(requestBody);
            
            //  <MessageModel> tar och bugga up som data object{"targetdeviceid": "consoleapp", "message":"dett är meddelandet"}

            targetDeviceId = targetDeviceId ?? data?.TargetDeviceId;
            message = message ?? data.Message;


            //köra f- async Task SendMessageToDeviceAsync(MAD.ServiceClient serviceClient, string targetDeviceId, string message)
            await DeviceService.SendMessageToDeviceAsync(serviceClient, targetDeviceId, message);
            // ++uppe " private static  readonly ServiceClient ;"


            return new OkResult(); //skicka tillbacka 200 ok
        }
    }
}