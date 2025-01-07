using UnityEngine;
using Zenject;

namespace _Scripts.Core.UICore.Page
{
	public abstract class PageInstaller : MonoInstaller
	{
	}

	public abstract class PageInstaller<View> : PageInstaller
		where View : BasePageView
	{
		[SerializeField]
		private View _viewPrefab;

		public override void InstallBindings()
		{
			Container
				.Bind<View>()
				.FromInstance(_viewPrefab)
				.AsSingle();
		}
	}

	public abstract class PageInstaller<View, ViewModel> : PageInstaller<View> 
		where View : BasePageView
	{
		public override void InstallBindings()
		{
			base.InstallBindings();
			Container
				.Bind<ViewModel>()
				.FromNew()
				.AsTransient();
		}
	}

	public abstract class PageInstaller<View, ViewModel, Model> : PageInstaller<View, ViewModel> 
		where View : BasePageView
	{
		public override void InstallBindings()
		{
			base.InstallBindings();
			Container
				.Bind<Model>()
				.FromNew()
				.AsTransient();
		}
	}
}
