using System.Collections.Generic;
using _Scripts.Apart.Extensions.Ecs.DelHere;
using _Scripts.Core.InputCore.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.InputCore
{
	public static class InputImplementationSystems
	{
		public static IEnumerable<IEcsSystem> GetPostStandardDelSystems()
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
