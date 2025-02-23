using _Scripts.Core.UICore.Page;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.CreateNewWorldPage
{
	public class CreateNewWorldView : BasePageView
	{
		[SerializeField]
		private TMP_InputField _worldNameInputField;
		[SerializeField]
		private Button _createNewWorldButton;
		[SerializeField]
		private Button _cancelButton;
		[SerializeField]
		private TextMeshProUGUI _worldNameAlreadyUsedText;

		[Inject]
		public CreateNewWorldViewModel ViewModel { get; private set; }

		private void Start()
		{
			_worldNameInputField.onValueChanged.AddListener(UpdateWorldNameInViewModel);
			_createNewWorldButton.onClick.AddListener(ViewModel.CreateAndLaunchNewWorld);
			_cancelButton.onClick.AddListener(Escape);

			ViewModel.WorldName.OnChanged += UpdateWorldNameInView;
			ViewModel.IsNameAlreadyUsed.OnChanged += SetWorldNameAlreadyUsed;

			UpdateWorldNameInViewModel(_worldNameInputField.text);
		}

		private void SetWorldNameAlreadyUsed(bool isUsed)
		{
			_worldNameAlreadyUsedText.gameObject.SetActive(isUsed);
			_createNewWorldButton.interactable = !isUsed;
		}

		private void UpdateWorldNameInViewModel(string worldName)
		{
			ViewModel.WorldName.Value = worldName;
		}

		private void UpdateWorldNameInView(string worldName)
		{
			_worldNameInputField.text = worldName;
		}
	}
}
