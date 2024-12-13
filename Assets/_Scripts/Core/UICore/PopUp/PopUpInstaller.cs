﻿using Assets.Scripts.Core.UICore.Core;
using Assets.Scripts.Core.UICore.Installers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Assets.Scripts.Core.UICore.PopUp
{
	public abstract class PopUpInstaller : MonoInstaller
	{

	}

	public abstract class PopUpInstaller<View> : PopUpInstaller
		where View : BasePopUpView
	{
		[SerializeField]
		private View _viewPrefab;

		public override void InstallBindings()
		{
			Container
				.Bind<View>()
				.FromInstance(_viewPrefab)
				.AsSingle();
		}
	}

	public abstract class PopUpInstaller<View, ViewModel> : PopUpInstaller<View>
		where View : BasePopUpView
	{
		public override void InstallBindings()
		{
			base.InstallBindings();
			Container
				.Bind<ViewModel>()
				.FromNew()
				.AsTransient();
		}
	}

	public abstract class PopUpInstaller<View, ViewModel, Model> : PopUpInstaller<View, ViewModel>
		where View : BasePopUpView
	{
		public override void InstallBindings()
		{
			base.InstallBindings();
			Container
				.Bind<Model>()
				.FromNew()
				.AsTransient();
		}
	}
}
