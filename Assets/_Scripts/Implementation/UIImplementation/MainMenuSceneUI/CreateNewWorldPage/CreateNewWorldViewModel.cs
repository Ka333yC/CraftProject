using System;
using _Scripts.Core.UICore;
using _Scripts.Implementation.SceneManagement;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.CreateNewWorldPage
{
	public class CreateNewWorldViewModel
	{
		public readonly ReactiveProperty<string> WorldName = new ReactiveProperty<string>("");

		private readonly CreateNewWorldModel _model;

		[Inject]
		private ScenesLauncher _scenesLauncher;

		public event Action<bool> OnWorldNameAlreadyUsed;

		public CreateNewWorldViewModel(CreateNewWorldModel model)
		{
			_model = model;
			WorldName.OnChanged += OnWorldNameChanged;
		}

		private async void OnWorldNameChanged(string worldName)
		{
			worldName = _model.TrimStringFromWrongSymbols(worldName);
			var hasWorldWithName = await _model.HasWorldWithName(worldName);
			OnWorldNameAlreadyUsed?.Invoke(hasWorldWithName);
		}

		public async void CreateAndLaunchNewWorld()
		{
			var worldId = await _model.CreateWorld(_model.TrimStringFromWrongSymbols(WorldName.Value));
			await _scenesLauncher.LaunchGameWorld(worldId);
		}
	}
}
