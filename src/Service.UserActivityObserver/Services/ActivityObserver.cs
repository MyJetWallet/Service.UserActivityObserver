using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyJetWallet.Sdk.Authorization.NoSql;
using MyNoSqlServer.Abstractions;
using Service.ClientWallets.MyNoSql;
using Service.PersonalData.Grpc;
using Service.PersonalData.Grpc.Contracts;
using Service.UserActivityObserver.Domain.Models;

namespace Service.UserActivityObserver.Services
{
    public class ObserverService
    {
        private readonly ILogger<ObserverService> _logger;
        private readonly DateTime _startingTime;
        private readonly TimeSpan _startingTimeout = TimeSpan.FromSeconds(100);
        private readonly IPersonalDataServiceGrpc _personalDataServiceGrpc;
        private readonly IMyNoSqlServerDataWriter<UserActivityNoSqlEntity> _writer;
        public ObserverService(ILogger<ObserverService> logger, IMyNoSqlServerDataReader<RootSessionNoSqlEntity> sessionReader, IMyNoSqlServerDataReader<ClientWalletNoSqlEntity> walletReader, IPersonalDataServiceGrpc personalDataServiceGrpc, IMyNoSqlServerDataWriter<UserActivityNoSqlEntity> writer)
        {
            _logger = logger;
            _personalDataServiceGrpc = personalDataServiceGrpc;
            _writer = writer;
            sessionReader.SubscribeToUpdateEvents(SessionsUpdate, SessionsDelete);
            walletReader.SubscribeToUpdateEvents(WalletsUpdate, null);
            _startingTime = DateTime.UtcNow;
        }

        private async void SessionsUpdate(IReadOnlyList<RootSessionNoSqlEntity> sessions)
        {
            if (DateTime.UtcNow > _startingTime.Add(_startingTimeout))
            {
                var events = new List<UserActivity>();
                var personalDatas = 
                    (await _personalDataServiceGrpc.GetByIdsAsync(new GetByIdsRequest()
                    {
                        Ids = sessions.Select(e => e.TraderId).ToList()
                    })).PersonalDatas
                    .ToDictionary(key=>key.Id, value=>value);

                foreach (var session in sessions)
                {
                    var email = personalDatas[session.TraderId].Email;
                    events.Add(new UserActivity
                    {
                        EventId = Guid.NewGuid().ToString(),
                        ClientId = session.TraderId,
                        ClientEmail = email,
                        EventType = "Login/Registration",
                        Description = session.Description,
                        RecordTime = DateTime.UtcNow
                    });
                }

                await RecordEventsAndKeepLast(events);
            }
        }

        private async void SessionsDelete(IReadOnlyList<RootSessionNoSqlEntity> sessions)
        {
            var events = new List<UserActivity>();
            var personalDatas = 
                (await _personalDataServiceGrpc.GetByIdsAsync(new GetByIdsRequest()
                {
                    Ids = sessions.Select(e => e.TraderId).ToList()
                })).PersonalDatas
                .ToDictionary(key=>key.Id, value=>value);

            foreach (var session in sessions)
            {
                var email = personalDatas[session.TraderId].Email;
                events.Add(new UserActivity
                {
                    EventId = Guid.NewGuid().ToString(),
                    ClientId = session.TraderId,
                    ClientEmail = email,
                    EventType = "Logout",
                    Description = session.Description,
                    RecordTime = DateTime.UtcNow
                });
            }
            await RecordEventsAndKeepLast(events);
        }

        private async void WalletsUpdate(IReadOnlyList<ClientWalletNoSqlEntity> wallets)
        {
            var events = new List<UserActivity>();
            if (DateTime.UtcNow > _startingTime.Add(_startingTimeout))
            {
                var newWallets = wallets.Where(e =>
                    e.Wallets.All(t => t.CreatedAt >= DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(5))));
                if (newWallets.Any())
                {
                    var personalDatas = 
                        (await _personalDataServiceGrpc.GetByIdsAsync(new GetByIdsRequest()
                        {
                            Ids = wallets.Select(e => e.ClientId).ToList()
                        }))
                        .PersonalDatas
                        .ToDictionary(key=>key.Id, value=>value);

                    foreach (var wallet in newWallets)
                    {
                        var email = personalDatas[wallet.ClientId].Email;
                        events.Add(new UserActivity
                        {
                            EventId = Guid.NewGuid().ToString(),
                            ClientId = wallet.ClientId,
                            ClientEmail = email,
                            EventType = "Wallet creation",
                            Description = "Wallet creation",
                            RecordTime = DateTime.UtcNow
                        });
                    }
                    await RecordEventsAndKeepLast(events);
                }
            }
        }

        private async Task RecordEventsAndKeepLast(List<UserActivity> events)
        {
            await _writer.BulkInsertOrReplaceAsync(events.Select(UserActivityNoSqlEntity.Create));
            await _writer.CleanAndKeepLastRecordsAsync(UserActivityNoSqlEntity.GeneratePartitionKey(),
                Program.Settings.MaxStoredEvents);
        }

    }
}
