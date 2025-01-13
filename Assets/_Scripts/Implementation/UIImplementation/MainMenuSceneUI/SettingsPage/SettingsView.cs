using _Scripts.Apart.Extensions;
using _Scripts.Core.UICore.Page;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.SettingsPage
{
	public class SettingsView : BasePageView
	{
		[SerializeField]
		private Button _doneButton;
		[SerializeField]
		private Slider _loadingRangeSlider;
		[SerializeField]
		private TextMeshProUGUI _loadingRangeCountText;

		[Inject]
		public SettingsViewModel ViewModel { get; private set; }

		private void Start()
		{
			_loadingRangeSlider.onValueChanged.AddListinerWholeNumbers(ViewModel.SetLoadingRange);
			SetLoadingRangeSliderValue(ViewModel.LoadingRange.Value);
			SetLoadingRangeCountTextValue(ViewModel.LoadingRange.Value);
			_doneButton.onClick.AddListener(Escape);

			ViewModel.LoadingRange.OnChanged += SetLoadingRangeSliderValue;
			ViewModel.LoadingRange.OnChanged += SetLoadingRangeCountTextValue;
		}

		private void OnDestroy()
		{
			ViewModel.Dispose();	
		}

		private void SetLoadingRangeCountTextValue(int value)
		{
			_loadingRangeCountText.text = value.ToString();
		}

		private void SetLoadingRangeSliderValue(int value)
		{
			_loadingRangeSlider.SetValueWithoutNotify(value);
		}
	}
}
