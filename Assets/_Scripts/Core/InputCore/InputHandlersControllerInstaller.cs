using Input.MonoBehaviourHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Voody.UniLeo.Lite;
using Zenject;

namespace Assets._Scripts.Core.InputCore
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
