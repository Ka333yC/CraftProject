using Assets.Scripts.Core.EcsCore;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.Core
{
	public class EcsWorldInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			var world = new EcsWorld();
			// Создаю энтити с id = 0, чтобы была возможность иметь default значение
			world.GetPool<DefaultEntityComponent>().Add(world.NewEntity());
			Container
				.Bind<EcsWorld>()
				.FromInstance(world)
				.AsSingle();
		}
	}
}
