using Assets.Scripts.Apart.Extensions.Ecs;
using Assets.Scripts.Core.PlayerCore.ActiveSlot.Systems;
using Assets.Scripts.Core.PlayerCore.Components;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
