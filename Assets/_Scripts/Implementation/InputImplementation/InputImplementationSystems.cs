using System.Collections.Generic;
using _Scripts.Apart.Extensions.Ecs.DelHere;
using _Scripts.Implementation.InputImplementation.Components;
using Leopotam.EcsLite;

namespace _Scripts.Implementation.InputImplementation
{
	public static class InputImplementationSystems
	{
		public static IEnumerable<IEcsSystem> GetPostFixedDelSystems()
		{
			return new List<IEcsSystem>()
			{
				new DelHereSystem<WalkInputComponent>(),
				new DelHereSystem<JumpInputTag>(),
				new DelHereSystem<RotationInputComponent>(),
				new DelHereSystem<HoldInputComponent>(),
				new DelHereSystem<TapInputComponent>(),
			};
		}
	}
}
