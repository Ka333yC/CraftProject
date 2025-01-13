using _Scripts.Core.UICore.Page;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.GameWorldSceneUI.PausePage
{
	public class PauseView : BasePageView
	{
		[SerializeField]
		private Button _resumeButton;
		[SerializeField]
		private Button _backToMenuButton;

		[Inject]
		public PauseViewModel ViewModel { get; private set; }

		private void Start()
		{
			_resumeButton.onClick.AddListener(ResumeGame);
			_backToMenuButton.onClick.AddListener(ViewModel.OpenStartMenu);

			ViewModel.PauseGame();
		}

		private void ResumeGame()
		{
			ViewModel.ResumeGame();
			PageStack.CloseLastView();
		}
	}
}
