using System.Collections.Generic;
using System.Threading.Tasks;
using dss_common.Functional;
using nicts_probate_sqs_api.Models;

namespace nicts_probate_sqs_api.Services
{
    public interface IAwsQueueService
    {
        Task<Result> EnQueue(ProbateApplicationModel probateApplicationModel);
        Task<Result<List<ProbateApplicationModel>>> DeQueue();
        Task DeleteMessage(string receiptHandle);
    }
}