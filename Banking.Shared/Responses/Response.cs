using System.Runtime.Serialization;

namespace Banking.Shared.Responses
{
    [DataContract]
    public class Response<T>
    {
        [DataMember]
        public ResponseStatus ResponseStatus { get; set; } = ResponseStatus.Success;

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public T Item { get; set; }
    }
}
