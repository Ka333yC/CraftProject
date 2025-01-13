using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Implementation.UIImplementation.LoadingsBars
{
	[RequireComponent(typeof(Image))]
	public class ImageFillAmountLoadingBar : LoadingBar
	{
		private Image _image;

		private void Awake()
		{
			_image = GetComponent<Image>();
		}

		public override void UpdateProgress(float value)
		{
			_image.fillAmount = value;
		}
	}
}
