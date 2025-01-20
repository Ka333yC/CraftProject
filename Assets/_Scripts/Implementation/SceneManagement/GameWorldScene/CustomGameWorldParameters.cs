using System.IO;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _Scripts.Implementation.SceneManagement.GameWorldScene
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

#if UNITY_EDITOR
	[CustomEditor(typeof(CustomGameWorldParameters))]
	public class CustomGameWorldParametersButton : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			var customGameWorldParameters = (CustomGameWorldParameters)target;
			if(!GUILayout.Button("Delete world folder"))
			{
				return;
			}

			Directory.Delete(customGameWorldParameters.GetGameWorldParameters().WorldFolderPath, true);
		}
	}
#endif
}
