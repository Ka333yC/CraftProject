using _Scripts.Implementation.DataBaseImplementation.WorldsDataDB.Tables.WorldParametersTable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Implementation.SceneManagement
{
	[CreateAssetMenu(fileName = "CustomGameWorldParameters", menuName = "Game world parameters")]
	public class CustomGameWorldParameters : ScriptableObject
	{
		[SerializeField]
		private int _id = -1;
		[SerializeField]
		private string _name = "custom_world";
		[SerializeField]
		private int _seed = 0;

		public GameWorldParameters GetGameWorldParameters()
		{
			var worldParameters = new GameWorldParameters();
			worldParameters.Id = _id;
			worldParameters.Name = _name;
			worldParameters.Seed = _seed;
			worldParameters.WorldFolderPath = Path.Combine(Application.persistentDataPath, worldParameters.Name);
			return worldParameters;
		}
	}
}
