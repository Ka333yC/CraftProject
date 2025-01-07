using UnityEngine;
using Zenject;

namespace _Scripts.Core.PhysicsCore.Presets
{
	public class PhysicsPresetsInstaller : MonoInstaller
	{
		[SerializeField]
		private PhysicsPresets _physicsPresets;

		public override void InstallBindings()
		{
			Container
				.Bind<PhysicsPresets>()
				.FromInstance(_physicsPresets)
				.AsSingle();
		}
	}
}
