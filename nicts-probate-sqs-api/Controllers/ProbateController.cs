using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json;
using nicts_probate_sqs_api.Models;

namespace nicts_probate_sqs_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProbateController : ControllerBase
    {
        private readonly IAmazonSQS _sqsClient;

        public ProbateController(IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
        }

        [HttpPost]
        [Route("Queue")]
        public async Task<IActionResult> SendToQueue([FromBody] ProbateApplicationModel applicationModel)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = "",
                MessageBody = JsonConvert.SerializeObject(applicationModel)
            };

            var sendMessageResponse = await _sqsClient.SendMessageAsync(sendMessageRequest);

            return sendMessageResponse == null ? (IActionResult) BadRequest() : Ok();
        }
    }
}
