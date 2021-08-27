using System.ServiceModel;
using System.Threading.Tasks;
using Service.UserActivityObserver.Grpc.Models;

namespace Service.UserActivityObserver.Grpc
{
    [ServiceContract]
    public interface IActivityService
    {
        [OperationContract]
        Task<ActivityListResponse> GetActivityEvents();
    }
}