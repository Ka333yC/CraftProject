using UnityEngine.InputSystem;

namespace Assets.Scripts.Core.InputCore.MonoBehaviourInputHandlers
{
	public interface IInputHandler
	{
		public void Initailize(InputActionMap actionMap);
		public void Enable();
		public void Disable();
	}
}
