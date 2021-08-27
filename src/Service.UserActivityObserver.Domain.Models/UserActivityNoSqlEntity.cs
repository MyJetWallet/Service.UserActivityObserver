using System;
using MyNoSqlServer.Abstractions;

namespace Service.UserActivityObserver.Domain.Models
{
    public class UserActivityNoSqlEntity : MyNoSqlDbEntity
    {
        public const string TableName = "user-activity-events";
        public static string GeneratePartitionKey() => "user-events";
        public static string GenerateRowKey(string eventId) => eventId;
        public UserActivity UserActivity { get; set; }

        public static UserActivityNoSqlEntity Create(UserActivity userActivity)
        {
            return new()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(userActivity.EventId),
                UserActivity = userActivity
            };
        }
    }
}