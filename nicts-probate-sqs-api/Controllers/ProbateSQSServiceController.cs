using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dss_common.Extensions;
using nicts_probate_sqs_api.PaaS;

namespace nicts_probate_sqs_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProbateSQSServiceController : ControllerBase
    {
        private readonly IProbateSQSService _probateSqsService;

        public ProbateSQSServiceController(IProbateSQSService probateSqsService)
        {
            _probateSqsService = probateSqsService;
        }

        [HttpGet]
        [Route("ListQueues")]
        public async Task<IActionResult> ListQueues() =>
            (await _probateSqsService.ListQueues()).OnBoth(r =>
                r.IsSuccess ? (IActionResult)Ok(r.Value) : BadRequest(r.Error));

        [HttpPost]
        [Route("Send")]
        public async Task<IActionResult> SendToQueue([FromForm] string applicationModel) =>
            (await _probateSqsService.SendMessage(applicationModel)).OnBoth(r =>
                r.IsSuccess ? (IActionResult)Ok() : BadRequest(r.Error));

        [HttpGet]
        [Route("ListMessages")]
        public async Task<IActionResult> ListMessages() =>
            (await _probateSqsService.ListMessages()).OnBoth(r =>
                r.IsSuccess ? (IActionResult)Ok(r.Value) : BadRequest(r.Error));
    }
}
