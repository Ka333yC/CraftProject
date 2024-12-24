using Assets.Scripts.Undone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.Implementation
{
	public class GameWorldDBInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container
				.BindInterfacesAndSelfTo<GameWorldDBCommandExecutor>()
				.FromNew()
				.AsSingle();
		}
	}
}
