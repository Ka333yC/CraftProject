using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using static UnityEngine.UI.Slider;

namespace Assets.Scripts.Apart.Extensions
{
	public static class SliderEventExtensions
	{
		public static void AddListinerWholeNumbers(this SliderEvent sliderEvent, Action<int> action)
		{
			sliderEvent.AddListener((value) => action?.Invoke((int)value));
		}
	}
}
