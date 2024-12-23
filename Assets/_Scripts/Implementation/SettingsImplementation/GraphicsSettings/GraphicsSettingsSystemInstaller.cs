using Assets.Scripts.Undone.SettingsCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.Implementation.Settings.GraphicsSettings
{
	public class GraphicsSettingsSystemInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container
				.BindInterfacesAndSelfTo<GraphicsSettingsSystem>()
				.FromNew()
				.AsSingle();
		}
	}
}
