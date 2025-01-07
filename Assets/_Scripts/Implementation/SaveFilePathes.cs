using System.IO;
using UnityEngine;

namespace _Scripts.Implementation
{
	public class SaveFilePathes
	{
		public static readonly string PathToWorldsDataDBFile = Path.Combine(Application.persistentDataPath, "worlds.db");
		public static readonly string PathToSaveFolder = Path.Combine(Application.persistentDataPath, "SaveData");
		public static readonly string GameWorldDatabaseFileName = "world.db";
	}
}
