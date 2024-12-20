using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Undone.WorldsCore
{
	public class SaveFilePathes
	{
		public static readonly string PathToWorldsDataDBFile = Path.Combine(Application.persistentDataPath, "worlds.db");
		public static readonly string PathToSaveFolder = Path.Combine(Application.persistentDataPath, "SaveData");
		public static readonly string GameWorldDatabaseFileName = "world.db";
	}
}
