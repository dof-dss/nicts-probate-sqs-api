using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using dss_common.Functional;
using Microsoft.Extensions.Options;
using nicts_probate_sqs_api.Models;

namespace nicts_probate_sqs_api.PaaS
{
    public class ProbateSQSService: IProbateSQSService
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly QueueModel _queueModel;

        public ProbateSQSService(IAmazonSQS sqsClient, IOptions<QueueModel> options)
        {
            _sqsClient = sqsClient;
            _queueModel = options?.Value;
        }

        public async Task<Result<List<string>>> ListQueues()
        {
            var listQueuesResponse = await _sqsClient.ListQueuesAsync(new ListQueuesRequest
            {
                MaxResults = 10
            });

            return listQueuesResponse.HttpStatusCode == HttpStatusCode.OK
                ? Result.Ok(listQueuesResponse.QueueUrls)
                : Result.Fail<List<string>>(listQueuesResponse.HttpStatusCode.ToString());
        }

        public async Task<Result> SendMessage(string messageBody)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = _queueModel.Url,
                MessageBody = messageBody
            };

            var sendMessageResponse = await _sqsClient.SendMessageAsync(sendMessageRequest);

            return sendMessageResponse == null ? Result.Fail("Message failed to send") : Result.Ok();
        }

        public async Task<Result<List<string>>> ListMessages()
        {
            var receiveMessageResponse =
                await _sqsClient.ReceiveMessageAsync(_queueModel.Url);
            if (receiveMessageResponse == null) Result.Fail("Failed to retrieve messages.");

            var messages = new List<string>();
            foreach (var message in receiveMessageResponse.Messages)
            {
                messages.Add(message.Body);
            }

            return Result.Ok(messages);
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
