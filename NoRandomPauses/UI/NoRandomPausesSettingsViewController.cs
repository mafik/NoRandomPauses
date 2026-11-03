using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;

namespace NoRandomPauses.UI
{
    [ViewDefinition("NoRandomPauses.UI.Settings.bsml")]
    internal class NoRandomPausesSettingsViewController : BSMLAutomaticViewController
    {
        [UIValue("enabled")]
        private bool Enabled
        {
            get => PluginConfig.Instance.Enabled;
            set => PluginConfig.Instance.Enabled = value;
        }

        [UIValue("suppression-ms")]
        private int SuppressionMs
        {
            get => PluginConfig.Instance.SuppressionMs;
            set => PluginConfig.Instance.SuppressionMs = value;
        }

        [UIAction("format-ms")]
        private string FormatMs(int value)
        {
            return $"{value} ms";
        }
    }
}
