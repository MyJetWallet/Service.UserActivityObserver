using MyJetWallet.Sdk.Service;
using MyYamlParser;

namespace Service.UserActivityObserver.Settings
{
    public class SettingsModel
    {
        [YamlProperty("UserActivityObserver.SeqServiceUrl")]
        public string SeqServiceUrl { get; set; }

        [YamlProperty("UserActivityObserver.ZipkinUrl")]
        public string ZipkinUrl { get; set; }

        [YamlProperty("UserActivityObserver.ElkLogs")]
        public LogElkSettings ElkLogs { get; set; }
        
        [YamlProperty("UserActivityObserver.MyNoSqlReaderHostPort")]
        public string MyNoSqlReaderHostPort { get; set; }
        
        [YamlProperty("UserActivityObserver.AuthMyNoSqlReaderHostPort")]
        public string AuthMyNoSqlReaderHostPort { get; set; }
        
        [YamlProperty("UserActivityObserver.PersonalDataServiceUrl")]
        public string PersonalDataServiceUrl { get; set; }
        
        [YamlProperty("UserActivityObserver.MyNoSqlWriterUrl")]
        public string MyNoSqlWriterUrl { get; set; }
        
        [YamlProperty("UserActivityObserver.MaxStoredEvents")]
        public int MaxStoredEvents { get; set; }

    }
}
