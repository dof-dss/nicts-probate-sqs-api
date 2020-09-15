using System;

namespace nicts_probate_sqs_api.Models
{
    public class StatementOfTruth
    {
        public DateTime DateSigned { get; set; }
        public string Statement { get; set; }
    }
}