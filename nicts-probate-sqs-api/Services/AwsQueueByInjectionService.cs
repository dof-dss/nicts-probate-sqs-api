using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using dss_common.Functional;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using nicts_probate_sqs_api.Models;

namespace nicts_probate_sqs_api.Services
{
    /// <summary>
    /// Configuration of queue is set up in Startup and injected into this class
    /// </summary>
    public class AwsQueueByInjectionService : BaseAwsQueueService
    {
        public AwsQueueByInjectionService(IAmazonSQS sqsClient, IOptions<QueueModel> options)
        {
            _sqsClient = sqsClient;
            _queueModel = options?.Value;
        }

    }
}
