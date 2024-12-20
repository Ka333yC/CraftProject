using Assets._Scripts.Undone.WorldsCore;
using Assets.Scripts.Core.ChunkCore.Saving;
using DataBaseManagement;
using Mono.Data.Sqlite;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Core.WorldsCore
{
	public class WorldsDataDBCommandExecutor : IDisposable
	{
		public readonly DataBaseCommandExecutor CommandExecutor = new DataBaseCommandExecutor();

		public WorldsDataDBCommandExecutor()
		{
			CommandExecutor.OpenConnection(SaveFilePathes.PathToWorldsDataDBFile);
			CommandExecutor.ExecuteNonQuery(WorldParameters.CreateTableCommand);
		}

		public void Dispose()
		{
			CommandExecutor.Dispose();
		}
	}
}
