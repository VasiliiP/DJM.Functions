using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DJM.Functions
{
    public static class DJMProducerFunction
    {
        [FunctionName("ClaimAuthorized")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [ServiceBus("claimauthorized", Connection = "ServiceBusConnection")] ServiceBusSender  sender,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var message = new ServiceBusMessage(requestBody);
            await sender.ScheduleMessageAsync(message, DateTimeOffset.UtcNow.AddDays(14));
            return new OkObjectResult("message sent");
        }
    }
}
