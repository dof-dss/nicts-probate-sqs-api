using System;

namespace nicts_probate_sqs_api.Models
{
    public class DocumentModel
    {
        public string DocumentType { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}