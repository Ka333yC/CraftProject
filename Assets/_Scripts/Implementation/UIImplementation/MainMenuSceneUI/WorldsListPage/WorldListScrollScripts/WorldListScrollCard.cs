using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldsListPage.WorldListScrollScripts
{
	[RequireComponent(typeof(Toggle))]
	public class WorldListScrollCard : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI _worldNameText;
		[SerializeField]
		private TextMeshProUGUI _creationTimeText;
		[SerializeField]
		private TextMeshProUGUI _lastPlayTimeText;

		[Inject]
		private GameWorldsDBCommandExecutor _commandExecutor;

		private GameWorldParameters _worldParameters;

		public event Action<int, bool> OnValueChanged;

		private void Awake()
		{
			var toggle = GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(InvokeOnToggleValueChanged);
		}

		private void OnDestroy()
		{
			OnValueChanged?.Invoke(_worldParameters.Id.Value, false);
		}

		public void SetWorld(GameWorldParameters worldParameters)
		{
			_worldParameters = worldParameters;
			SetWorldName(worldParameters.Name);
			var creationTime = Directory.GetCreationTime(worldParameters.WorldFolderPath);
			SetCreationTime(creationTime);
			var lastWriteTime = Directory.GetLastWriteTime(worldParameters.WorldFolderPath);
			SetLastPlayTime(lastWriteTime);
		}
		
		private void SetWorldName(string value)
		{
			_worldNameText.text = value;
		}

		private void SetCreationTime(DateTime value)
		{
			_creationTimeText.text = value.ToString();
		}

		private void SetLastPlayTime(DateTime value)
		{
			_lastPlayTimeText.text = value.ToString();
		}

		private void InvokeOnToggleValueChanged(bool value)
		{
			OnValueChanged?.Invoke(_worldParameters.Id.Value, value);
		}
	}
}
