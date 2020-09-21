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
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using nicts_probate_sqs_api.Models;

namespace nicts_probate_sqs_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProbateController : ControllerBase
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly QueueModel _queueModel;

        public ProbateController(IAmazonSQS sqsClient, IOptions<QueueModel> options)
        {
            _sqsClient = sqsClient;
            _queueModel = options?.Value;
        }

        [HttpPost]
        [Route("Queue")]
        public async Task<IActionResult> SendToQueue([FromBody] ProbateApplicationModel applicationModel)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = _queueModel.Url,
                MessageBody = JsonConvert.SerializeObject(applicationModel)
            };

            var sendMessageResponse = await _sqsClient.SendMessageAsync(sendMessageRequest);

            return sendMessageResponse == null ? (IActionResult) BadRequest() : Ok();
        }

        [HttpGet]
        [Route("Queue")]
        public async Task<IActionResult> DeQueue()
        {
            var receiveMessageResponse =
                await _sqsClient.ReceiveMessageAsync(_queueModel.Url);

            return receiveMessageResponse == null ? (IActionResult)BadRequest() 
                : Ok(JsonConvert.DeserializeObject<ProbateApplicationModel>(receiveMessageResponse?.Messages?.First()?.Body));
        }
    }
}
