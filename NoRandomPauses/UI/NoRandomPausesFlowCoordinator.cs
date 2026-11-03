using System;
using HMUI;
using Zenject;

namespace NoRandomPauses.UI
{
    internal class NoRandomPausesFlowCoordinator : FlowCoordinator
    {
        [Inject]
        private readonly NoRandomPausesSettingsViewController _settingsViewController = null;

        public event Action DidFinish;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("No Random Pauses");
                showBackButton = true;
            }
            if (addedToHierarchy)
            {
                ProvideInitialViewControllers(_settingsViewController);
            }
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            DidFinish?.Invoke();
        }
    }
}
