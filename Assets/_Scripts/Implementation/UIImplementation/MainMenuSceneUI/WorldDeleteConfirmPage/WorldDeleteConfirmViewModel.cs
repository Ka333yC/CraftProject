using System;
using _Scripts.Core.UICore;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldDeleteConfirmPage
{
	public class WorldDeleteConfirmViewModel
	{
		public readonly ReactiveProperty<int> WorldId = new ReactiveProperty<int>();

		private WorldDeleteConfirmModel _model;

		public event Action<int> OnWorldDeleted;

		public WorldDeleteConfirmViewModel(WorldDeleteConfirmModel model)
		{
			_model = model;
		}

		public async void DeleteWorld()
		{
			await _model.DeleteWorld(WorldId.Value);
			OnWorldDeleted?.Invoke(WorldId.Value);
		}
	}
}
