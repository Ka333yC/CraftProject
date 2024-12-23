using Assets.Scripts.PhysicsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.PhysicsPresetsScripts
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
