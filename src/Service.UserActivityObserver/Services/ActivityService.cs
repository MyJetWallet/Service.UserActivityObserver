using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyNoSqlServer.Abstractions;
using Service.UserActivityObserver.Domain.Models;
using Service.UserActivityObserver.Grpc;
using Service.UserActivityObserver.Grpc.Models;

namespace Service.UserActivityObserver.Services
{
    public class ActivityService: IActivityService
    {
        private readonly ILogger<ActivityService> _logger;
        private readonly IMyNoSqlServerDataWriter<UserActivityNoSqlEntity> _writer;

        public ActivityService(ILogger<ActivityService> logger, IMyNoSqlServerDataWriter<UserActivityNoSqlEntity> writer)
        {
            _logger = logger;
            _writer = writer;
        }
        
        public async Task<ActivityListResponse> GetActivityEvents()
        {
            var entities = await _writer.GetAsync();
            return new ActivityListResponse()
            {
                Activities = entities.Select(e => e.UserActivity).OrderByDescending(e=>e.RecordTime).ToList()
            };
        }
    }
    
}