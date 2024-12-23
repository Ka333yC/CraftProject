using Assets.Scripts.Undone.SettingsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Implementation.Settings.GraphicsSettings
{
	public class GraphicsSettingsSystem : ISettingsSystem
	{
		private int _loadingRange = 0;

		public int LoadingRange
		{
			get => _loadingRange;
			set
			{
				if(value < 2)
				{
					throw new ArgumentException("Loading Range cannot be less then 2");
				}

				_loadingRange = value;
			}
		}

		public GraphicsSettingsSystem(SettingsSystemsManager settingsSystemsManager)
		{
			settingsSystemsManager.AddSystem(this);
		}

		public void GetFrom(SettingsData settingsData)
		{
			if(settingsData.TryGetInt(nameof(LoadingRange), out var loadingRange))
			{
				LoadingRange = loadingRange;
			}
			else
			{
				LoadingRange = 8;
			}
		}

		public void SetTo(SettingsData settingsData)
		{
			settingsData.SetInt(nameof(LoadingRange), LoadingRange);
		}
	}
}
