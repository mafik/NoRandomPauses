using System;
using BeatSaberMarkupLanguage.MenuButtons;
using Zenject;

namespace NoRandomPauses.UI
{
    internal class NoRandomPausesMenuButton : IInitializable, IDisposable
    {
        private readonly MenuButtons _menuButtons;
        private readonly MainFlowCoordinator _mainFlowCoordinator;
        private readonly NoRandomPausesFlowCoordinator _flowCoordinator;
        private readonly MenuButton _menuButton;

        public NoRandomPausesMenuButton(MenuButtons menuButtons, MainFlowCoordinator mainFlowCoordinator,
            NoRandomPausesFlowCoordinator flowCoordinator)
        {
            _menuButtons = menuButtons;
            _mainFlowCoordinator = mainFlowCoordinator;
            _flowCoordinator = flowCoordinator;
            _menuButton = new MenuButton("No Random Pauses", Present);
        }

        public void Initialize()
        {
            _flowCoordinator.DidFinish += Dismiss;
            _menuButtons.RegisterButton(_menuButton);
        }

        public void Dispose()
        {
            _flowCoordinator.DidFinish -= Dismiss;
            _menuButtons.UnregisterButton(_menuButton);
        }

        private void Present()
        {
            _mainFlowCoordinator.PresentFlowCoordinator(_flowCoordinator);
        }

        private void Dismiss()
        {
            _mainFlowCoordinator.DismissFlowCoordinator(_flowCoordinator);
        }
    }
}
