using System;
using System.Runtime.Serialization;

namespace Banking.Shared.Responses
{
    [DataContract]
    public class TimeSeriesDataPoint<T>
    {
        [DataMember]
        public DateTime Timestamp { get; set; }

        [DataMember]
        public T Value { get; set; }
    }
}
