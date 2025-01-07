using UnityEngine;
using Zenject;

namespace _Scripts.Core.InputCore
{
	public class InputHandlersManagerInstaller : MonoInstaller
	{
		[SerializeField]
		private InputHandlersManager _inputHandlersManager;

		public override void InstallBindings()
		{
			Container
				.Bind<InputHandlersManager>()
				.FromInstance(_inputHandlersManager)
				.AsSingle();
		}
	}
}
