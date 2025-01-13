using _Scripts.Core.UICore.Page;
using _Scripts.Implementation.UIImplementation.MainMenuSceneUI.SettingsPage;
using _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldsListPage;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.MainMenuPage
{
	public class MainMenuView : BasePageView
	{
		[SerializeField]
		private Button _singleGameButton;
		[SerializeField]
		private Button _settingsButton;

		private void Start()
		{
			_singleGameButton.onClick.AddListener(OpenWorldsListView);
			_settingsButton.onClick.AddListener(OpenSettingsView);
		}

		public override void Escape()
		{

		}

		private void OpenWorldsListView()
		{
			var worldsListView = ViewFactory.CreatePage<WorldsListView>();
			PageStack.OpenView(worldsListView);
		}

		private void OpenSettingsView()
		{
			var settingsView = ViewFactory.CreatePage<SettingsView>();
			PageStack.OpenView(settingsView);
		}
	}
}
