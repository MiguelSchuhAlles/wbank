using System.Runtime.Serialization;

namespace Banking.Shared.Requests
{
    [DataContract]
    public class PaymentRequestDTO : OperationRequestDTO
    {
        [DataMember]
        public string Code { get; set; }
    }
}
