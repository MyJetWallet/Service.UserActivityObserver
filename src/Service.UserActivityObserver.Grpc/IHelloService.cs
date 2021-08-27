using System.ServiceModel;
using System.Threading.Tasks;
using Service.UserActivityObserver.Grpc.Models;

namespace Service.UserActivityObserver.Grpc
{
    [ServiceContract]
    public interface IHelloService
    {
        [OperationContract]
        Task<HelloMessage> SayHelloAsync(HelloRequest request);
    }
}