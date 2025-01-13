using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

		private CancellationTokenSource _token = new CancellationTokenSource();
		private Task _fillTask = Task.CompletedTask;
		private Dictionary<int, WorldListScrollCard> _usedCards =
			new Dictionary<int, WorldListScrollCard>();
		private LinkedList<int> _worldsIdToCreate = new LinkedList<int>();
		private LinkedList<int> _worldsIdToDelete = new LinkedList<int>();
		private ScrollRect _scrollRect;

		public event Action<int> OnCardSelected;
		public event Action<int> OnCardDeselected;

		private void Awake()
		{
			_scrollRect = GetComponent<ScrollRect>();
		}

		private void Update()
		{
			if(_worldsIdToCreate.Any() && _fillTask.IsCompleted)
			{
				var worldIdToCreate = _worldsIdToCreate.First();
				_worldsIdToCreate.RemoveFirst();
				_fillTask = CreateAndFillCard(worldIdToCreate);
			}

			if(_worldsIdToDelete.Any())
			{
				var worldIdToRecord = _worldsIdToDelete.First();
				_worldsIdToDelete.RemoveFirst();
				DeleteCard(worldIdToRecord);
			}
		}

		private void OnDestroy()
		{
			_token.Cancel();
			_token.Dispose();
		}

		public void AddToCreate(int worldId)
		{
			_worldsIdToCreate.AddLast(worldId);
		}

		public void AddToDelete(int worldId)
		{
			if(_worldsIdToCreate.Remove(worldId))
			{
				return;
			}

			_worldsIdToDelete.AddLast(worldId);
		}

		private async Task CreateAndFillCard(int worldId)
		{
			try
			{
				var createdElement =
					_diContainer.InstantiatePrefab(_elementPrefab).GetComponent<WorldListScrollCard>();
				createdElement.transform.SetParent(_scrollRect.content, false);
				createdElement.OnValueChanged += CardValueChanged;
				_usedCards.Add(worldId, createdElement);
				await createdElement.SetWorld(worldId, _token.Token);
				createdElement.gameObject.SetActive(true);
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
			var element = _usedCards[worldId];
			Destroy(element.gameObject);
			_usedCards.Remove(worldId);
		}
	}
}
