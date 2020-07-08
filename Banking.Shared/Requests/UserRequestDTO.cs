using System.Runtime.Serialization;

namespace Banking.Shared.Requests
{
    public class UserRequestDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember(IsRequired = true)]
        public bool Enabled { get; set; } = true;
    }
}
