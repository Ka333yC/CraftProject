using _Scripts.Core.UICore;
using _Scripts.Core.UICore.Page;
using _Scripts.Implementation.UIImplementation.MainMenuSceneUI.MainMenuPage;
using UnityEngine;
using Zenject;

namespace _Scripts.Undone.StartPoints
{
	public class MainMenuSceneStartPoint : MonoBehaviour
	{
		[Inject]
		private ViewFactory _viewFactory;
		[Inject]
		private PageViewStack _viewStack;

		private void Start()
		{
			var mainMenu = _viewFactory.CreatePage<MainMenuView>();
			_viewStack.OpenView(mainMenu);
		}
	}
}
