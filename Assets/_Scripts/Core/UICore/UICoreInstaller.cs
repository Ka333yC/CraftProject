using System.Collections.Generic;
using _Scripts.Core.UICore.Page;
using _Scripts.Core.UICore.PopUp;
using UnityEngine;
using Zenject;

namespace _Scripts.Core.UICore
{
	public class UICoreInstaller : MonoInstaller
	{
		[SerializeField]
		private RootCanvas _rootCanvas;
		[SerializeField]
		private List<PageInstaller> _pageInstallers;
		[SerializeField]
		private List<PopUpInstaller> _popUpInstallers;

		public override void InstallBindings()
		{
			Container
				.Bind<ViewFactory>()
				.FromNew()
				.AsSingle();
			Container
				.Bind<PageViewStack>()
				.FromNew()
				.AsSingle();
			Container
				.Bind<RootCanvas>()
				.FromInstance(_rootCanvas)
				.AsSingle();
			InstallPages();
			InstallPopUps();
		}

		private void InstallPages()
		{
			foreach(var pageInstaller in _pageInstallers)
			{
				Container.Inject(pageInstaller);
				pageInstaller.InstallBindings();
			}
		}

		private void InstallPopUps()
		{
			foreach(var popUpInstaller in _popUpInstallers)
			{
				Container.Inject(popUpInstaller);
				popUpInstaller.InstallBindings();
			}
		}
	}
}
