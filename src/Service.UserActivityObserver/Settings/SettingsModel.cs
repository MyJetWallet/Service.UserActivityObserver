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
    }
}
