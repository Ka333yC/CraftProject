﻿using System;
using System.Collections.Generic;
using _Scripts.Core.SettingsCore;
using UnityEngine;

namespace _Scripts.Implementation.SettingsImplementation.GraphicsSettings
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

		public void LoadFrom(SettingsData settingsData)
		{
			if(settingsData.TryGet(nameof(LoadingRange), out int loadingRange))
			{
				LoadingRange = loadingRange;
			}
			else
			{
				LoadingRange = 8;
			}
		}

		public void SaveTo(SettingsData settingsData)
		{
			settingsData.Set(nameof(LoadingRange), LoadingRange);
		}
	}
}
