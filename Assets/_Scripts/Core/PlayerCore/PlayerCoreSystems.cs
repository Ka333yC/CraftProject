using Leopotam.EcsLite;
using System.Collections.Generic;
using Assets.Scripts.Core.PlayerCore.ActiveSlot.Systems;
using Assets.Scripts.Apart.Extensions.Ecs.DelHere;
using Assets.Scripts.Core.PlayerCore.Components;
using Assets.Scripts.Core.PlayerCore.ChunkSet.Systems;
using Assets.Scripts.Undone.PlayerCore.Saving.Systems;
using Assets.Scripts.Apart.Extensions.Ecs;

namespace Assets.Scripts.Core.PlayerCore
{
	public static class PlayerCoreSystems
	{
		public static IEnumerable<IEcsSystem> GetFixedSystems()
		{
			return new List<IEcsSystem>()
			{
#region 
				new UpdateChunkUsersAroundPlayersSystem(),
#endregion
			};
		}
	}
}
