using System;
using _Scripts.Core.UICore;
using _Scripts.Implementation.SceneManagement;
using R3;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.CreateNewWorldPage
{
	public class CreateNewWorldViewModel
	{
		public readonly ReactiveProperty<string> WorldName = new ("");
		public readonly ReactiveProperty<bool> IsNameAlreadyUsed = new (false);

		private readonly CreateNewWorldModel _model;

		[Inject]
		private ScenesLauncher _scenesLauncher;

		public CreateNewWorldViewModel(CreateNewWorldModel model)
		{
			_model = model;
			WorldName.Subscribe(UpdateIsNameAlreadyUsed);
		}

		private async void UpdateIsNameAlreadyUsed(string worldName)
		{
			worldName = _model.TrimStringFromWrongSymbols(worldName);
			IsNameAlreadyUsed.Value = await _model.HasWorldWithName(worldName);
		}

		public async void CreateAndLaunchNewWorld()
		{
			var worldName = _model.TrimStringFromWrongSymbols(WorldName.Value);
			var worldId = await _model.CreateWorld(worldName);
			await _scenesLauncher.LaunchGameWorld(worldId);
		}
	}
}
