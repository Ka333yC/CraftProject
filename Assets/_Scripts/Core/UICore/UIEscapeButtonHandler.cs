using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.UICore.Core
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
