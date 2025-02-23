using System;
using R3;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldDeleteConfirmPage
{
	public class WorldDeleteConfirmViewModel
	{
		public readonly ReactiveProperty<int> WorldId = new ReactiveProperty<int>();

		private readonly WorldDeleteConfirmModel _model;

		public event Action<int> OnWorldDeleted;

		public WorldDeleteConfirmViewModel(WorldDeleteConfirmModel model)
		{
			_model = model;
		}

		public async void DeleteWorld()
		{
			_model.WorldId = WorldId.Value;
			await _model.DeleteWorld();
			OnWorldDeleted?.Invoke(WorldId.Value);
		}
	}
}
