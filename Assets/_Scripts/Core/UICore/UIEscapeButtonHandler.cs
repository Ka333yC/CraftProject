using System.Linq;
using _Scripts.Core.UICore.Page;
using UnityEngine;
using Zenject;

namespace _Scripts.Core.UICore
{
	public class UIEscapeButtonHandler : MonoBehaviour
	{
		[Inject]
		private PageViewStack _pageViewStack;

		private void Update()
		{
			if(UnityEngine.Input.GetKeyDown(KeyCode.Escape))
			{
				var lastPage = _pageViewStack.LastOrDefault();
				lastPage?.Escape();
			}
		}
	}
}
