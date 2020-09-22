using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using dss_common.Functional;
using Microsoft.Extensions.Options;
using nicts_probate_sqs_api.Models;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace nicts_probate_sqs_api.Services
{
    /// <summary>
    /// Accesses configuration via the Options pattern provided by SteelToe
    /// and sets up SQS Client in constructor
    /// </summary>
    public class QueueByConfigurationService: BaseQueueService
    {
        public QueueByConfigurationService(IOptions<QueueModel> options,
            IOptions<CloudFoundryApplicationOptions> appOptions,
            IOptions<CloudFoundryServicesOptions> serviceOptions)
        {
            var accessKey = serviceOptions.Value.Services["user-provided"].First().Credentials["AccessKey"].Value;
            var secretKey = serviceOptions.Value.Services["user-provided"].First().Credentials["SecretKey"].Value;
            var region = serviceOptions.Value.Services["user-provided"].First().Credentials["Region"].Value;

            _sqsClient = new AmazonSQSClient(new BasicAWSCredentials(accessKey, secretKey), RegionEndpoint.GetBySystemName(region));

            _queueModel = options?.Value;
        }
    }
}
