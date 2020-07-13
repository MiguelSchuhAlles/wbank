using System.Runtime.Serialization;

namespace Banking.Shared.Requests
{
    [DataContract]
    public class TransferenceRequestDTO: OperationRequestDTO
    {
        [DataMember(IsRequired = true)]
        public bool SameTitularity { get; set; }

        [DataMember]
        public string RecipientName { get; set; }

        [DataMember]
        public string RecipientCode { get; set; }

        [DataMember(IsRequired = true)]
        public string RecipientAccountCode { get; set; }

        [DataMember(IsRequired = true)]
        public int RecipientBranchNumber { get; set; }

        [DataMember(IsRequired = true)]
        public int RecipientBankNumber { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
