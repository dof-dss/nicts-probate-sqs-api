using System.Collections.Generic;
using System.Threading.Tasks;
using dss_common.Functional;

namespace nicts_probate_sqs_api.PaaS
{
    public interface IProbateSQSService
    {
        Task<Result<List<string>>> ListQueues();
        Task<Result> SendMessage(string messageBody);
        Task<Result<List<string>>> ListMessages();
    }
}