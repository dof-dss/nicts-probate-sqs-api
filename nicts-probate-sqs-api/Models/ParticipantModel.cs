using System;

namespace nicts_probate_sqs_api.Models
{
    public class ParticipantModel
    {
        public string ParticipantType { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public bool HandlingEstate { get; set; }
        public bool Reserved { get; set; }
        public bool InformedInWriting { get; set; }
        public bool Renunciation { get; set; }
        public string OtherName { get; set; }
        public DateTime DateOfDeath { get; set; }
        public string SolicitorFirmName { get; set; }
        public string SolicitorFirmAddress { get; set; }
    }
}