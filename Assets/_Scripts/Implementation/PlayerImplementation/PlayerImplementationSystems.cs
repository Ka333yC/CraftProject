using System.Collections.Generic;
using _Scripts.Apart.Extensions.Ecs.DelHere;
using _Scripts.Implementation.PlayerImplementation.Input.Systems;
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
#region Сохранение
				// Сохранение
				new MarkChangedPlayersNeedToSaveSystem(),
				new PlayerSaveSystem(),
#endregion
#region Обработка ввода
				new PlayerRotationProcessSystem(),
				new PlayerWalkProcessSystem(),
				new PlayerJumpProcessSystem(),
				new PlayerInteractionProcessSystem(),
			};
#endregion
		}
		
		public static IEnumerable<IEcsSystem> GetStandardSystems()
		{
			return new List<IEcsSystem>()
			{
#region Inventory
				// new DelHereSystem<ActiveSlotChangedComponent>(),
				// new ChangePlayerActiveSlotSystem(),
#endregion
			};
		}
	}
}
