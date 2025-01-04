using System;
using Zenject;

namespace Assets.Scripts.Core.WorldsCore
{
	public class WorldsDataInstaller : MonoInstaller 
	{
		public override void InstallBindings()
		{
			Container
				.BindInterfacesAndSelfTo<WorldLauncher>()
				.FromNew()
				.AsSingle();
			Container
				.BindInterfacesAndSelfTo<WorldsDataDBCommandExecutor>()
				.FromNew()
				.AsSingle();
		}
	}
}
