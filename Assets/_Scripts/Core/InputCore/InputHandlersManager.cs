using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace _Scripts.Core.InputCore
{
	public class InputHandlersManager : MonoBehaviour
	{
		private readonly List<IInputHandler> _inputHandlers = new List<IInputHandler>();

		[SerializeField] 
		private InputActionAsset _playerControls;
		[SerializeField] 
		private string _actionMapName = "ActionMap";

		private bool _isInitialized;
		private InputActionMap _actionMap;

		private void Awake()
		{
			_actionMap = _playerControls.FindActionMap(_actionMapName);
			EnhancedTouchSupport.Enable();
			InitializeHandlers();
			_isInitialized = true;
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

		public void AddInputHandler(IInputHandler inputHandler)
		{
			_inputHandlers.Add(inputHandler);
			if(!_isInitialized)
			{
				return;
			}

			inputHandler.Initailize(_actionMap);
			if(enabled)
			{
				inputHandler.Enable();
			}
			else
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
