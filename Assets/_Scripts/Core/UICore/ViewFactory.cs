using Assets.Assets.Scripts.Core.UICore.PopUp;
using System;
using Zenject;

namespace Assets.Scripts.Core.UICore.Core
{
	public class ViewFactory
	{
		[Inject]
		private DiContainer _container;

		public T CreatePage<T>() where T : BasePageView
		{
			var viewPrefab = _container.Resolve<T>();
			var viewGO = _container.InstantiatePrefab(viewPrefab);
			var view = viewGO.GetComponent<T>();
			return view;
		}

		public T CreatePopUp<T>() where T : BasePopUpView
		{
			var viewPrefab = _container.Resolve<T>();
			var viewGO = _container.InstantiatePrefab(viewPrefab);
			var view = viewGO.GetComponent<T>();
			return view;
		}
	}
}
