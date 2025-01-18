using System.Collections.Generic;
using _Scripts.Implementation.PlayerImplementation.Input.Systems;
using _Scripts.Implementation.PlayerImplementation.Movement.Systems;
using _Scripts.Implementation.PlayerImplementation.PlayerSerialization.Systems;
using Input.Systems;
using Leopotam.EcsLite;

namespace _Scripts.Implementation.PlayerImplementation
{
	public static class PlayerImplementationSystems
	{
		public static IEnumerable<IEcsSystem> GetFixedSystems()
		{
			return new List<IEcsSystem>()
			{
				// Сохранение
				new MarkChangedPlayersNeedToSaveSystem(),
				new PlayerSaveSystem(),
				// Обработка ввода
				new PlayerRotationProcessSystem(),
				new PlayerWalkProcessSystem(),
				new PlayerJumpProcessSystem(),
				new PlayerInteractionProcessSystem(),
			};
		}
	}
}
