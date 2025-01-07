using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace _Scripts.Core.InputCore
{
	public class InputHandlersController : MonoBehaviour
	{
		[SerializeField] 
		private InputActionAsset _playerControls;
		[SerializeField] 
		private string _actionMapName = "ActionMap";

		private InputActionMap _actionMap;
		private IInputHandler[] _inputHandlers;

		private void Awake()
		{
			_actionMap = _playerControls.FindActionMap(_actionMapName);
			_inputHandlers = GetComponents<IInputHandler>();
			EnhancedTouchSupport.Enable();
			InitializeHandlers();
		}

		private void OnEnable()
		{
			foreach(var inputHandler in _inputHandlers)
			{
				inputHandler.Enable();
			}
		}

		private void OnDisable()
		{
			foreach(var inputHandler in _inputHandlers)
			{
				inputHandler.Disable();
			}
		}

		public T GetInputHandler<T>() where T : IInputHandler
		{
			var result = _inputHandlers
				.Where(inputHandler => inputHandler is T)
				.FirstOrDefault();
			return (T)result;
		}

		private void InitializeHandlers()
		{
			foreach(var inputHandler in _inputHandlers)
			{
				inputHandler.Initailize(_actionMap);
			}
		}
	}
}
