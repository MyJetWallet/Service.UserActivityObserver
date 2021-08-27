using System.Runtime.Serialization;
using Service.UserActivityObserver.Domain.Models;

namespace Service.UserActivityObserver.Grpc.Models
{
    [DataContract]
    public class HelloMessage : IHelloMessage
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }
    }
}