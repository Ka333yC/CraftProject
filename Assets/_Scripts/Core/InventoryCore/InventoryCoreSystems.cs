using Assets.Scripts.Apart.Extensions.Ecs;
using Assets.Scripts.Core.PlayerCore.ActiveSlot.Systems;
using Assets.Scripts.Core.PlayerCore.Components;
using Leopotam.EcsLite;
using System.Collections.Generic;

namespace Assets.Scripts.Core.InventoryCore
{
	public static class InventoryCoreSystems
	{
		public static IEnumerable<IEcsSystem> GetStandartSystems()
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
