using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Scripts.Undone.WorldsCore
{
    public class ScenesLauncher
    {
        public GameWorldLaunchParameters? GameWorldLaunchParameters { get; private set; }
        
        public async UniTask LaunchGameWorld(int worldId)
        {
            var gameWorldLaunchParameters = new GameWorldLaunchParameters();
			gameWorldLaunchParameters.WorldId = worldId;
			GameWorldLaunchParameters = gameWorldLaunchParameters;
			await SceneManager.LoadSceneAsync(ScenesIndexes.GameWorldSceneIndex).ToUniTask();
        }

        public async UniTask LaunchMainMenu()
        {
            GameWorldLaunchParameters = null;
            await SceneManager.LoadSceneAsync(ScenesIndexes.MainMenuSceneIndex).ToUniTask();
        }
    }
}