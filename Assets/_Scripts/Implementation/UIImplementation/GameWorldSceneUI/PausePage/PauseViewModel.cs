namespace _Scripts.Implementation.UIImplementation.GameWorldSceneUI.PausePage
{
	public class PauseViewModel
	{
		private readonly PauseModel _model;

		public PauseViewModel(PauseModel model)
		{
			_model = model;
		}

		public void PauseGame()
		{
			_model.PauseGame();
		}

		public void ResumeGame()
		{
			_model.ResumeGame();
		}

		public async void OpenStartMenu()
		{
			_model.ResumeGame();
			await _model.OpenStartMenuScene();
		}
	}
}
