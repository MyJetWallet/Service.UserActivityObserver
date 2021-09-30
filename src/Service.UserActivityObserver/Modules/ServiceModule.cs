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
            var noSqlClient = builder.CreateNoSqlClient(Program.ReloadedSettings(e => e.MyNoSqlReaderHostPort));
            var authNoSqlClient = builder.CreateNoSqlClient(Program.ReloadedSettings(e => e.AuthMyNoSqlReaderHostPort));
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