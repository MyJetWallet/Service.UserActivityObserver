using JetBrains.Annotations;
using MyJetWallet.Sdk.Grpc;
using Service.UserActivityObserver.Grpc;

namespace Service.UserActivityObserver.Client
{
    [UsedImplicitly]
    public class UserActivityObserverClientFactory: MyGrpcClientFactory
    {
        public UserActivityObserverClientFactory(string grpcServiceUrl) : base(grpcServiceUrl)
        {
        }

        public IHelloService GetHelloService() => CreateGrpcService<IHelloService>();
    }
}
