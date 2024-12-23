using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.ChunkCore.Saving;
using Assets.Scripts.Core.PlayerCore;
using DataBaseManagement;
using Leopotam.EcsLite;
using PhysicsCore.ObjectPhysics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Undone.PlayerCore.Saving
{
	public class PlayersLoader
	{
		private readonly DataBaseCommandExecutor _commandExecutor;
		private readonly EcsWorld _world;
		private readonly EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;

		public PlayersLoader(GameWorldDBCommandExecutor commandExecutor, EcsWorld world)
		{
			_commandExecutor = commandExecutor.CommandExecutor;
			_world = world;
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();
		}

		public async Task Save(int playerEntity, CancellationToken token)
		{
			var objectPhysics = _objectPhysicsPool.Get(playerEntity);
			var playerInDatabase = new PlayerInDatabase();
			playerInDatabase.Nickname = "Steve";
			playerInDatabase.SetPosition(objectPhysics.Rigidbody.position);
			await WriteToDatabase(playerInDatabase);
		}

		public async Task<Vector3?> GetSavedPosition(CancellationToken token) 
		{
			Vector3? result = null;
			var selectCommand =
				PlayerInDatabase.SelectWhereNicknameCommand;
			selectCommand.Nickname = "Steve";
			await _commandExecutor.ExecuteReaderAsync(selectCommand, (reader) =>
			{
				if(!reader.Read())
				{
					return;
				}

				token.ThrowIfCancellationRequested();
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
