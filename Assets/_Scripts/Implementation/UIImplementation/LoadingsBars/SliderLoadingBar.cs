using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Implementation.UIImplementation.LoadingsBars
{
	[RequireComponent(typeof(Slider))]
	public class SliderLoadingBar : LoadingBar
	{
		private Slider _slider;

		private void Awake()
		{
			_slider = GetComponent<Slider>();
		}

		public override void UpdateProgress(float value)
		{
			_slider.value = value;
		}
	}
}
