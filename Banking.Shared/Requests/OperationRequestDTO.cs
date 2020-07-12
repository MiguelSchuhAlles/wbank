using System.Runtime.Serialization;

namespace Banking.Shared.Requests
{
    [DataContract]
    public class OperationRequestDTO
    {
        [DataMember]
        public int AccountId { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public decimal Amount { get; set; }
    }
}
