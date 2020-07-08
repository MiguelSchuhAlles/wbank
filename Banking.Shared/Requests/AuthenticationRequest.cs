using System.Runtime.Serialization;

namespace Banking.Shared.Requests
{
    [DataContract]
    public class AuthenticationRequest
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}
