using _Scripts.Core.UICore;
using _Scripts.Core.UICore.Page;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Undone
{
	public class StartMenuSceneStartPoint : MonoBehaviour
	{
		[Inject]
		private ViewFactory _viewFactory;
		[Inject]
		private PageViewStack _viewStack;

		private void Start()
		{
			// var mainMenu = _viewFactory.CreatePage<MainMenuView>();
			// _viewStack.OpenView(mainMenu);
		}
	}
}
