using System.Threading.Tasks;
using _Scripts.Core.Pause;
using _Scripts.Implementation.SceneManagement;
using UnityEngine.SceneManagement;

namespace _Scripts.Implementation.UIImplementation.GameWorldSceneUI.PausePage
{
	public class PauseModel
	{
		private readonly PauseManager _pauseManager;

		public PauseModel(PauseManager pauseManager)
		{
			_pauseManager = pauseManager;
		}

		public void PauseGame()
		{
			_pauseManager.IsPaused = true;
		}

		public void ResumeGame()
		{
			_pauseManager.IsPaused = false;
		}

		public async Task OpenStartMenuScene()
		{
			var asyncOperation = SceneManager.LoadSceneAsync(ScenesIndexes.MainMenuSceneIndex);
			while(!asyncOperation.isDone)
			{
				await Task.Yield();
			}
		}
	}
}
