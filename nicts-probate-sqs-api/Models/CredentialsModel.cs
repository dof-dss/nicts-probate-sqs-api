using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nicts_probate_sqs_api.Models
{
    public class CredentialsModel
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Region { get; set; }
    }
}
