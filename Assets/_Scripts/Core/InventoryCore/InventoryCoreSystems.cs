using System.Collections.Generic;
using _Scripts.Apart.Extensions.Ecs.DelHere;
using _Scripts.Core.InventoryCore.Components;
using _Scripts.Core.InventoryCore.Systems;
using Leopotam.EcsLite;

namespace _Scripts.Core.InventoryCore
{
	public static class InventoryCoreSystems
	{
		public static IEnumerable<IEcsSystem> GetStandardSystems()
		{
			return new List<IEcsSystem>()
			{
#region 
				new DelHereSystem<ActiveSlotChangedComponent>(),
				new ChangeActiveSlotSystem(),
#endregion
			};
		}
	}
}
