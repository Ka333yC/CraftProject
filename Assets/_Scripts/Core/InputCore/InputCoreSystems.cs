using Leopotam.EcsLite;
using System.Collections.Generic;
using Input.Components;
using Assets.Scripts.Apart.Extensions.Ecs;

namespace Assets.Scripts.Core.InputCore
{
	public static class InputCoreSystems
	{
		public static IEnumerable<IEcsSystem> GetPostStandartDelSystems()
		{
			return new List<IEcsSystem>()
			{
				new DelHereSystem<WalkInputComponent>(),
				new DelHereSystem<JumpInputTag>(),
				new DelHereSystem<LookInputComponent>(),
				new DelHereSystem<HoldInputComponent>(),
				new DelHereSystem<TapInputComponent>(),
			};
		}
	}
}
