using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using dss_common.Functional;
using Newtonsoft.Json;
using nicts_probate_sqs_api.Models;

namespace nicts_probate_sqs_api.Services
{
    /// <summary>
    /// Base wrapper class for AWS SQS Client
    /// </summary>
    public class BaseQueueService: IQueueService
    {
        protected IAmazonSQS _sqsClient;
        protected QueueModel _queueModel;

        public async Task<Result> EnQueue(ProbateApplicationModel probateApplicationModel)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = _queueModel.Url,
                MessageBody = JsonConvert.SerializeObject(probateApplicationModel)
            };

            var sendMessageResponse = await _sqsClient.SendMessageAsync(sendMessageRequest);

            return sendMessageResponse == null ? Result.Fail("Message failed to send") : Result.Ok();
        }

        public async Task<Result<List<ProbateApplicationModel>>> DeQueue()
        {
            var receiveMessageResponse =
                await _sqsClient.ReceiveMessageAsync(_queueModel.Url);
            if (receiveMessageResponse == null) Result.Fail("Failed to retrieve messages.");

            var probateApplications = new List<ProbateApplicationModel>();
            foreach (var message in receiveMessageResponse.Messages)
            {
                probateApplications.Add(JsonConvert.DeserializeObject<ProbateApplicationModel>(message.Body));
                await DeleteMessage(message.ReceiptHandle).ConfigureAwait(false);

            }

            return Result.Ok(probateApplications);
        }

        public async Task DeleteMessage(string receiptHandle)
        {
            var deleteMessageRequest = new DeleteMessageRequest
            {
                QueueUrl = _queueModel.Url,
                ReceiptHandle = receiptHandle
            };

            await _sqsClient.DeleteMessageAsync(deleteMessageRequest).ConfigureAwait(false);
        }
    }
}
