using System;
using System.Runtime.Serialization;

namespace Service.UserActivityObserver.Domain.Models
{
    [DataContract]
    public class UserActivity
    {
        [DataMember(Order = 1)]public string EventId { get; set; }
        [DataMember(Order = 2)]public string ClientId { get; set; }
        [DataMember(Order = 3)]public string ClientEmail { get; set; }
        [DataMember(Order = 4)]public string EventType { get; set; }
        [DataMember(Order = 5)]public string Description { get; set; }
        [DataMember(Order = 6)]public DateTime RecordTime { get; set; }
    }
}