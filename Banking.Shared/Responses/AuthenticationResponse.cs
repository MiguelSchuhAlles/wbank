using System.Runtime.Serialization;

namespace Banking.Shared.Responses
{
    [DataContract]
    public class AuthenticationResponse
    {
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Token { get; set; }
    }
}
