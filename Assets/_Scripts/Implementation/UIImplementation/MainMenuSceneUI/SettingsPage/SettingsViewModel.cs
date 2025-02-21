using System;
using _Scripts.Core.SettingsCore;
using _Scripts.Core.UICore;
using _Scripts.Implementation.SettingsImplementation.GraphicsSettings;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.SettingsPage
{
	public class SettingsViewModel : IDisposable
	{
		public readonly ReactiveProperty<int> LoadingRange;

		private readonly SettingsSystemsManager _settingsManager;
		private readonly GraphicsSettingsSystem _graphicsSettingsSystem;

		public SettingsViewModel(SettingsSystemsManager settingsManager, 
			GraphicsSettingsSystem graphicsSettingsSystem)
		{
			_settingsManager = settingsManager;
			_graphicsSettingsSystem = graphicsSettingsSystem;
			LoadingRange = new ReactiveProperty<int>(_graphicsSettingsSystem.LoadingRange);
		}

		public void Dispose()
		{
			
		}

		public void SetLoadingRange(int loadingRange)
		{
			_graphicsSettingsSystem.LoadingRange = loadingRange;
			LoadingRange.Value = loadingRange;
		}
	}
}
