using System.Collections.Generic;
using _Scripts.Core.PlayerCore.ChunkSet.Systems;
using Leopotam.EcsLite;

namespace _Scripts.Core.PlayerCore
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
