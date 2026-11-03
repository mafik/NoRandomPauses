using Zenject;

namespace NoRandomPauses.Installers
{
    internal class NoRandomPausesMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<UI.NoRandomPausesSettingsViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<UI.NoRandomPausesFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
            Container.BindInterfacesTo<UI.NoRandomPausesMenuButton>().AsSingle();
        }
    }
}
