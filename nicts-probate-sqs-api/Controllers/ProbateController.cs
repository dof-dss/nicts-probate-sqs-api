using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using dss_common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using nicts_probate_sqs_api.Models;
using nicts_probate_sqs_api.Services;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace nicts_probate_sqs_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProbateController : ControllerBase
    {
        private readonly IQueueService _queueService;

        public ProbateController(IQueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpPost]
        [Route("Queue")]
        public async Task<IActionResult> SendToQueue([FromBody] ProbateApplicationModel applicationModel) => 
            (await _queueService.EnQueue(applicationModel)).OnBoth(r => 
                r.IsSuccess ? (IActionResult)Ok() : BadRequest(r.Error));

        [HttpGet]
        [Route("Queue")]
        public async Task<IActionResult> DeQueue() =>
            (await _queueService.DeQueue()).OnBoth(r =>
                r.IsSuccess ? (IActionResult) Ok(r.Value) : BadRequest(r.Error));
    }
}
