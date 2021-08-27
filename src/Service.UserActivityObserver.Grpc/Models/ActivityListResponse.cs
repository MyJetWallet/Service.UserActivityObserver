using System.Collections.Generic;
using System.Runtime.Serialization;
using Service.UserActivityObserver.Domain.Models;

namespace Service.UserActivityObserver.Grpc.Models
{
    [DataContract]
    public class ActivityListResponse
    {
        [DataMember(Order = 1)] public List<UserActivity> Activities { get; set; }
    }
}