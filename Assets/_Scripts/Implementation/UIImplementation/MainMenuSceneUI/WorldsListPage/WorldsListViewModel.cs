using System;
using System.Collections.ObjectModel;
using System.Threading;
using _Scripts.Core.UICore;
using _Scripts.Implementation.SceneManagement;
using Unity.VisualScripting;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldsListPage
{
	public class WorldsListViewModel : IDisposable
	{
		public readonly ObservableCollection<int> WorldsId = new ObservableCollection<int>();
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
			WorldsId.AddRange(await _model.LoadWorldsId(_cts.Token));
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
