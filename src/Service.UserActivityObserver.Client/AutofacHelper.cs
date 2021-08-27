using Autofac;
using Service.UserActivityObserver.Grpc;

// ReSharper disable UnusedMember.Global

namespace Service.UserActivityObserver.Client
{
    public static class AutofacHelper
    {
        public static void RegisterUserActivityObserverClient(this ContainerBuilder builder, string grpcServiceUrl)
        {
            var factory = new UserActivityObserverClientFactory(grpcServiceUrl);

            builder.RegisterInstance(factory.GetHelloService()).As<IHelloService>().SingleInstance();
        }
    }
}
