using System.Reflection;
using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using NoRandomPauses.Installers;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace NoRandomPauses
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static IPALogger Log { get; private set; }

        private Harmony _harmony;

        [Init]
        public Plugin(IPALogger logger, Config config, Zenjector zenjector)
        {
            Log = logger;
            PluginConfig.Instance = config.Generated<PluginConfig>();
            zenjector.Install<NoRandomPausesMenuInstaller>(Location.Menu);
        }

        [OnStart]
        public void OnApplicationStart()
        {
            _harmony = new Harmony("NoRandomPauses");
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnExit]
        public void OnApplicationQuit()
        {
            _harmony?.UnpatchSelf();
        }
    }
}
