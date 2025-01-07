using System.Threading.Tasks;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Implementation.DataBaseImplementation.GameWorldDB;
using _Scripts.Implementation.DataBaseImplementation.GameWorldDB.Tables.PlayerInDatabaseTable;
using DataBaseManagement;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Implementation.PlayerImplementation.PlayerSerialization
{
	public class PlayersSerializer
	{
		private const string _nickname = "Steve";

		private readonly EcsWorld _world;
		private readonly EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;

		private readonly DataBaseCommandExecutor _commandExecutor;

		public PlayersSerializer(GameWorldDBCommandExecutor commandExecutor, EcsWorld world)
		{
			_world = world;
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();

			_commandExecutor = commandExecutor.CommandExecutor;
		}

		public async Task Save(int playerEntity)
		{
			var objectPhysics = _objectPhysicsPool.Get(playerEntity);
			var playerInDatabase = new PlayerInDatabase();
			playerInDatabase.Nickname = _nickname;
			playerInDatabase.SetPosition(objectPhysics.Rigidbody.position);
			await WriteToDatabase(playerInDatabase);
		}

		public async Task<Vector3?> GetSavedPosition() 
		{
			Vector3? result = null;
			var selectCommand =
				PlayerInDatabase.SelectWhereNicknameCommand;
			selectCommand.Nickname = _nickname;
			await _commandExecutor.ExecuteReaderAsync(selectCommand, (reader) =>
			{
				if(!reader.Read())
				{
					return;
				}

				float x = reader.GetFloat(0);
				float y = reader.GetFloat(1);
				float z = reader.GetFloat(2);
				result = new Vector3(x, y, z);
			}).ConfigureAwait(false);

			return result;
		}

		private async Task WriteToDatabase(PlayerInDatabase player)
		{
			var insertOrReplaceCommand = PlayerInDatabase.InsertOrReplaceCommand;
			insertOrReplaceCommand.Player = player;
			await _commandExecutor.ExecuteNonQueryAsync(insertOrReplaceCommand);
		}
	}
}
