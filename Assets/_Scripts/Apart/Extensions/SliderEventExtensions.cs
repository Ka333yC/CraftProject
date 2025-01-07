using System;
using static UnityEngine.UI.Slider;

namespace _Scripts.Apart.Extensions
{
	public static class SliderEventExtensions
	{
		public static void AddListinerWholeNumbers(this SliderEvent sliderEvent, Action<int> action)
		{
			sliderEvent.AddListener((value) => action?.Invoke((int)value));
		}
	}
}
