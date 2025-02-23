using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldsListPage.WorldListScrollScripts
{
	[RequireComponent(typeof(ScrollRect))]
	public class WorldListScroll : MonoBehaviour
	{
		[SerializeField]
		private WorldListScrollCard _elementPrefab;

		[Inject]
		private DiContainer _diContainer;

		private readonly LinkedList<GameWorldParameters> _worldsParametersToCreateCards = new ();
		private readonly Dictionary<int, WorldListScrollCard> _createdCards = new ();
		private ScrollRect _scrollRect;

		public event Action<int> OnCardSelected;
		public event Action<int> OnCardDeselected;

		private void Awake()
		{
			_scrollRect = GetComponent<ScrollRect>();
		}

		private void Update()
		{
			if(_worldsParametersToCreateCards.Any())
			{
				var worldIdToCreate = _worldsParametersToCreateCards.First();
				_worldsParametersToCreateCards.RemoveFirst();
				CreateAndFillCard(worldIdToCreate);
			}
		}

		public void AddToCreate(GameWorldParameters worldParameters)
		{
			_worldsParametersToCreateCards.AddLast(worldParameters);
		}

		public void Delete(int worldId)
		{
			var worldToCreate = _worldsParametersToCreateCards
				.FirstOrDefault(worldParameter => worldParameter.Id.Value == worldId);
			if(worldToCreate != null)
			{
				_worldsParametersToCreateCards.Remove(worldToCreate);
				return;
			}
			
			DeleteCard(worldId);
		}

		private void CreateAndFillCard(GameWorldParameters worldParameters)
		{
			try
			{
				var createdElement =
					_diContainer.InstantiatePrefab(_elementPrefab).GetComponent<WorldListScrollCard>();
				createdElement.transform.SetParent(_scrollRect.content, false);
				createdElement.OnValueChanged += CardValueChanged;
				createdElement.SetWorld(worldParameters);
				createdElement.gameObject.SetActive(true);
				_createdCards.Add(worldParameters.Id.Value, createdElement);
			}
			catch(OperationCanceledException)
			{
			}
		}

		private void CardValueChanged(int worldId, bool value)
		{
			if(value)
			{
				OnCardSelected?.Invoke(worldId);
			}
			else
			{
				OnCardDeselected?.Invoke(worldId);
			}
		}

		private void DeleteCard(int worldId)
		{
			var element = _createdCards[worldId];
			Destroy(element.gameObject);
			_createdCards.Remove(worldId);
		}
	}
}
