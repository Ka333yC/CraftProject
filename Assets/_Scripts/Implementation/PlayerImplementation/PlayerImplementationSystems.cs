using Assets.Scripts.Core.PlayerCore.ChunkSet.Systems;
using Assets.Scripts.Undone.PlayerCore.Saving.Systems;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Implementation.PlayerImplementation
{
	public static class PlayerImplementationSystems
	{
		public static IEnumerable<IEcsSystem> GetFixedSystems()
		{
			return new List<IEcsSystem>()
			{
#region 
				new MarkChangedPlayersNeedToSaveSystem(),
				new PlayerSaveSystem(),
#endregion
			};
		}
	}
}
