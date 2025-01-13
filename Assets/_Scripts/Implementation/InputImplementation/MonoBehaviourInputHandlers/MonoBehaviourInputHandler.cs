using _Scripts.Core.InputCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets._Scripts.Implementation.InputImplementation.MonoBehaviourInputHandlers
{
	public abstract class MonoBehaviourInputHandler : MonoBehaviour, IInputHandler
	{
		[Inject]
		private InputHandlersManager _inputHandlersManager;

		protected virtual void Awake()
		{
			_inputHandlersManager.AddInputHandler(this);
		}

		public abstract void Initailize(InputActionMap actionMap);
		public abstract void Enable();
		public abstract void Disable();
	}
}
