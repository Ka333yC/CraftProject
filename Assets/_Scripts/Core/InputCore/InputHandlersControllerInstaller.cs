using UnityEngine;
using Zenject;

namespace _Scripts.Core.InputCore
{
	public class InputHandlersControllerInstaller : MonoInstaller
	{
		[SerializeField]
		private InputHandlersController _inputHandlersController;

		public override void InstallBindings()
		{
			Container
				.Bind<InputHandlersController>()
				.FromInstance(_inputHandlersController)
				.AsSingle();
		}
	}
}
