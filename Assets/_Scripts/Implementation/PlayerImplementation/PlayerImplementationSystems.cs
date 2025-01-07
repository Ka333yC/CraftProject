using System.Collections.Generic;
using _Scripts.Implementation.PlayerImplementation.PlayerSerialization.Systems;
using Leopotam.EcsLite;

namespace _Scripts.Implementation.PlayerImplementation
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
