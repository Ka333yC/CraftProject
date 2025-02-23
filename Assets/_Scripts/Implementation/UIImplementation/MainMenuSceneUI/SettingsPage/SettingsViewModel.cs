using System;
using _Scripts.Core.SettingsCore;
using _Scripts.Core.UICore;
using _Scripts.Implementation.SettingsImplementation.GraphicsSettings;
using R3;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.SettingsPage
{
	public class SettingsViewModel
	{
		public readonly ReactiveProperty<int> LoadingRange;

		private readonly GraphicsSettingsSystem _graphicsSettingsSystem;

		public SettingsViewModel(GraphicsSettingsSystem graphicsSettingsSystem)
		{
			_graphicsSettingsSystem = graphicsSettingsSystem;
			LoadingRange = new ReactiveProperty<int>(_graphicsSettingsSystem.LoadingRange);
		}

		public void SetLoadingRange(int loadingRange)
		{
			_graphicsSettingsSystem.LoadingRange = loadingRange;
			LoadingRange.Value = loadingRange;
		}
	}
}
