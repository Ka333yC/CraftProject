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

		private int _worldId;

		public event Action<int, bool> OnValueChanged;

		private void Awake()
		{
			var toggle = GetComponent<Toggle>();
			toggle.onValueChanged.AddListener(InvokeOnToggleValueChanged);
		}

		private void OnDestroy()
		{
			OnValueChanged?.Invoke(_worldId, false);
		}

		public async Task SetWorld(int worldId, CancellationToken token)
		{
			_worldId = worldId;
			var worldSettings = await GetWorldSettings(worldId, token);
			SetWorldName(worldSettings.Name);
			var creationTime = Directory.GetCreationTime(worldSettings.WorldFolderPath);
			SetCreationTime(creationTime);
			var lastWriteTime = Directory.GetLastWriteTime(worldSettings.WorldFolderPath);
			SetLastPlayTime(lastWriteTime);
		}

		private async Task<GameWorldParameters> GetWorldSettings(int worldId, CancellationToken token) 
		{
			var worldSettings = new GameWorldParameters();
			var selectCommand = GameWorldParameters.SelectWhereIdCommand;
			selectCommand.Id = worldId;
			await _commandExecutor.CommandExecutor.ExecuteReaderAsync(selectCommand, (reader) =>
			{
				token.ThrowIfCancellationRequested();
				if(!reader.Read())
				{
					throw new ArgumentException("The request returned nothing.");
				}

				worldSettings.Name = reader.GetString(0);
				worldSettings.Seed = reader.GetInt32(1);
				worldSettings.WorldFolderPath = reader.GetString(2);
			});

			return worldSettings;
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
			OnValueChanged?.Invoke(_worldId, value);
		}
	}
}
