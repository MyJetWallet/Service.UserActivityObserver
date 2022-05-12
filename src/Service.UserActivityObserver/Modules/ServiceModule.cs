using Autofac;
using MyJetWallet.Sdk.Authorization.NoSql;
using MyJetWallet.Sdk.NoSql;
using Service.ClientWallets.MyNoSql;
using Service.PersonalData.Client;
using Service.UserActivityObserver.Domain.Models;
using Service.UserActivityObserver.Services;

namespace Service.UserActivityObserver.Modules
{
    public class ServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var noSqlClient = builder.CreateNoSqlClient(Program.Settings.MyNoSqlReaderHostPort, Program.LogFactory);
            var authNoSqlClient = builder.CreateNoSqlClient(Program.Settings.AuthMyNoSqlReaderHostPort, Program.LogFactory);
            builder.RegisterMyNoSqlReader<ClientWalletNoSqlEntity>(noSqlClient, ClientWalletNoSqlEntity.TableName);
            builder.RegisterMyNoSqlReader<RootSessionNoSqlEntity>(authNoSqlClient, RootSessionNoSqlEntity.TableName);
            builder.RegisterMyNoSqlWriter<UserActivityNoSqlEntity>(Program.ReloadedSettings(e => e.MyNoSqlWriterUrl),
                UserActivityNoSqlEntity.TableName);

            builder.RegisterPersonalDataClient(Program.Settings.PersonalDataServiceUrl);


            builder
                .RegisterType<ObserverService>()
                .AsSelf()
                .AutoActivate()
                .SingleInstance();
        }
    }
}