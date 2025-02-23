using System;
using System.Threading;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable;
using _Scripts.Implementation.SceneManagement;
using ObservableCollections;
using R3;
using Unity.VisualScripting;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldsListPage
{
	public class WorldsListViewModel : IDisposable
	{
		public readonly ObservableList<GameWorldParameters> Worlds = new ();
		public readonly ReactiveProperty<int> SelectedWorldId = new ReactiveProperty<int>(-1);

		private readonly WorldsListModel _model;
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();

		[Inject]
		private ScenesLauncher _worldLauncher;

		public WorldsListViewModel(WorldsListModel model)
		{
			_model = model;
		}

		public async void LoadData() 
		{
			Worlds.AddRange(await _model.LoadWorlds(_cts.Token));
		}

		public async void PlayInSelectedWorld()
		{
			await _worldLauncher.LaunchGameWorld(SelectedWorldId.Value);
		}

		public void Dispose()
		{
			_cts.Cancel();
			_cts.Dispose();
		}
	}
}
