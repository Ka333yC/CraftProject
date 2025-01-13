using _Scripts.Core.UICore.Page;
using _Scripts.Implementation.UIImplementation.GameWorldSceneUI.PausePage;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.GameWorldSceneUI.MainGameWorldPage
{
	public class MainGameWorldView : BasePageView
	{
		[SerializeField]
		private Button _pauseButton;

		[Inject]
		public MainGameWorldViewModel ViewModel { get; private set; }

		private void Start()
		{
			_pauseButton.onClick.AddListener(OpenPauseView);
		}

		public override void Escape()
		{
			OpenPauseView();
		}

		private void OpenPauseView() 
		{
			var pauseView = ViewFactory.CreatePage<PauseView>();
			PageStack.OpenView(pauseView);
		}
	}
}
