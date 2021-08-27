using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using MyJetWallet.Sdk.Authorization.NoSql;
using MyJetWallet.Sdk.Grpc;
using MyJetWallet.Sdk.NoSql;
using Service.ClientWallets.MyNoSql;
using Service.UserActivityObserver.Domain.Models;
using Service.UserActivityObserver.Services;
using SimpleTrading.PersonalData.Grpc;

namespace Service.UserActivityObserver.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var noSqlClient = builder.CreateNoSqlClient(Program.ReloadedSettings(e => e.MyNoSqlReaderHostPort));
            builder.RegisterMyNoSqlReader<RootSessionNoSqlEntity>(noSqlClient, RootSessionNoSqlEntity.TableName);
            builder.RegisterMyNoSqlReader<ClientWalletNoSqlEntity>(noSqlClient, ClientWalletNoSqlEntity.TableName);
            builder.RegisterMyNoSqlWriter<UserActivityNoSqlEntity>(Program.ReloadedSettings(e => e.MyNoSqlWriterUrl),
                UserActivityNoSqlEntity.TableName);
            var personalDataClientFactory = new MyGrpcClientFactory(Program.Settings.PersonalDataServiceUrl);

            builder
                .RegisterInstance(personalDataClientFactory.CreateGrpcService<IPersonalDataServiceGrpc>())
                .As<IPersonalDataServiceGrpc>()
                .SingleInstance();


            builder
                .RegisterType<ObserverService>()
                .AsSelf()
                .AutoActivate()
                .SingleInstance();
        }
    }
}