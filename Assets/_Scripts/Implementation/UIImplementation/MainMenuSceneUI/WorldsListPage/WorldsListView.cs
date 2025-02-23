using System;
using System.Collections.Specialized;
using _Scripts.Core.UICore.Page;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable;
using _Scripts.Implementation.UIImplementation.MainMenuSceneUI.CreateNewWorldPage;
using _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldDeleteConfirmPage;
using _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldsListPage.WorldListScrollScripts;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldsListPage
{
	public class WorldsListView : BasePageView
	{
		[SerializeField]
		private WorldListScroll _worldsListScroll;
		[SerializeField]
		private Button _playInSelectedWorldButton;
		[SerializeField]
		private Button _deleteSelectedWorldButton;
		[SerializeField]
		private Button _createNewWorldButton;
		[SerializeField]
		private Button _backButton;

		[Inject]
		public WorldsListViewModel ViewModel { get; private set; }

		private void Start()
		{
			_worldsListScroll.OnCardSelected += WorldListScrollOnCardSelected;
			_worldsListScroll.OnCardDeselected += WorldListScrollOnCardDeselected;
			_playInSelectedWorldButton.onClick.AddListener(ViewModel.PlayInSelectedWorld);
			_deleteSelectedWorldButton.onClick.AddListener(OpenWorldDeleteConfirmView);
			_createNewWorldButton.onClick.AddListener(OpenCreateNewWorldView);
			_backButton.onClick.AddListener(Escape);

			ViewModel.Worlds.CollectionChanged += WorldsIdCollectionChanged;
			ViewModel.LoadData();
		}

		private void OnDestroy()
		{
			ViewModel.Dispose();
		}

		private void OpenCreateNewWorldView()
		{
			var createNewWorldView = ViewFactory.CreatePage<CreateNewWorldView>();
			PageStack.OpenView(createNewWorldView);
		}

		private void OpenWorldDeleteConfirmView()
		{
			var worldDeleteConfirmView = ViewFactory.CreatePage<WorldDeleteConfirmView>();
			worldDeleteConfirmView.ViewModel.WorldId.Value = ViewModel.SelectedWorldId.Value;
			worldDeleteConfirmView.ViewModel.OnWorldDeleted += DeleteWorldFromList;
			PageStack.OpenView(worldDeleteConfirmView);
		}

		private void WorldsIdCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if(e.Action != NotifyCollectionChangedAction.Add)
			{
				throw new ArgumentException(e.Action.ToString());
			}

			foreach(var newItem in e.NewItems)
			{
				_worldsListScroll.AddToCreate((GameWorldParameters)newItem);
			}
		}

		private void DeleteWorldFromList(int worldId) 
		{
			_worldsListScroll.Delete(worldId);
		}

		private void WorldListScrollOnCardSelected(int worldId)
		{
			ViewModel.SelectedWorldId.Value = worldId;
			SetInteractableWorldInteractionButtons(true);
		}

		private void WorldListScrollOnCardDeselected(int worldId)
		{
			SetInteractableWorldInteractionButtons(false);
		}

		private void SetInteractableWorldInteractionButtons(bool interactable)
		{
			_playInSelectedWorldButton.interactable = interactable;
			_deleteSelectedWorldButton.interactable = interactable;
		}
	}
}
