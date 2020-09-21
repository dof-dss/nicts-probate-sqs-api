using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace nicts_probate_sqs_api.Models
{
    public class ProbateApplicationModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationType { get; set; }
        public bool HasOriginalWill { get; set; }
        public DateTime DateWillSigned { get; set; }
        public bool IHTSubmittedOnline { get; set; }
        public string IHTReference { get; set; }
        public decimal EstateGrossValue { get; set; }
        public decimal EstateNetValue { get; set; }
        public string DeceasedForename { get; set; }
        public string DeceasedSurname { get; set; }
        public string DeceasedAddress { get; set; }
        public DateTime DeceasedDoD { get; set; }
        public DateTime DeceasedDoB { get; set; }
        public string Domicile { get; set; }
        public bool HasAssetsInOtherName { get; set; }
        public string OtherName { get; set; }
        public int ExtraUKCopies { get; set; }
        public int ExtraSealedCopies { get; set; }
        public IEnumerable<ParticipantModel> Participants { get; set; }
        public IEnumerable<DocumentModel> Documents { get; set; }
        public StatementOfTruth StatementOfTruth { get; set; }
        public IEnumerable<Codicil> Codicils { get; set; }
    }
}