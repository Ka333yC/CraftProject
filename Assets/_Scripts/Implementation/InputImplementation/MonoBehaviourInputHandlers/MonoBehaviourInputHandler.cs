using _Scripts.Core.InputCore;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Scripts.Implementation.InputImplementation.MonoBehaviourInputHandlers
{
	public abstract class MonoBehaviourInputHandler : MonoBehaviour, IInputHandler
	{
		[Inject]
		private InputHandlersManager _inputHandlersManager;

		protected virtual void Awake()
		{
			_inputHandlersManager.AddInputHandler(this);
		}

		public abstract void Initialize(InputActionMap actionMap);
		public abstract void Enable();
		public abstract void Disable();
	}
}
