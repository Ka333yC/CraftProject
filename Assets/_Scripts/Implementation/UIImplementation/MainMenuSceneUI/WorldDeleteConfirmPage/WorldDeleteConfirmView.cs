using _Scripts.Core.UICore.Page;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldDeleteConfirmPage
{
	public class WorldDeleteConfirmView : BasePageView
	{
		[SerializeField]
		private Button _deleteButton;
		[SerializeField]
		private Button _cancelButton;

		[Inject]
		public WorldDeleteConfirmViewModel ViewModel { get; private set; }

		private void Start()
		{
			_deleteButton.onClick.AddListener(ViewModel.DeleteWorld);
			_cancelButton.onClick.AddListener(Escape);

			ViewModel.OnWorldDeleted += (worldId) => Escape();
		}
	}
}
