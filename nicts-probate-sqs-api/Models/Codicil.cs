using System;

namespace nicts_probate_sqs_api.Models
{
    public class Codicil
    {
        public DateTime DateSigned { get; set; }
        public DateTime DateUploaded { get; set; }
        public string URL { get; set; }
    }
}