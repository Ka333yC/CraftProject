using UnityEngine.InputSystem;

namespace _Scripts.Core.InputCore
{
	public interface IInputHandler
	{
		public void Initailize(InputActionMap actionMap);
		public void Enable();
		public void Disable();
	}
}
