using System;
using _Scripts.Core.PlayerCore;
using _Scripts.Core.PlayerCore.Components;
using _Scripts.Core.UICore.PopUp;
using _Scripts.Implementation.PlayerImplementation.PlayerInventory;
using _Scripts.Implementation.PlayerImplementation.PlayerInventory.Components;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace _Scripts.Implementation.UIImplementation.GameWorldSceneUI.PlayerToolbarPopUp
{
	public class PlayerToolbarPopUpView : BasePopUpView
	{
		[SerializeField]
		private ToolbarSlot[] _uiSlots = new ToolbarSlot[PlayerConstantData.ToolbarSize];

		[Inject]
		private EcsWorld _ecsWorld;
		
		private Toolbar _toolbar;
		private int _previousActiveSlotIndex;
		
		private void OnValidate()
		{
			if(_uiSlots.Length != PlayerConstantData.ToolbarSize)
			{
				Array.Resize(ref _uiSlots, PlayerConstantData.ToolbarSize);
			}
		}

		private void Start()
		{
			_toolbar = GetPlayerInventory().Toolbar;
			for(int i = 0; i < PlayerConstantData.ToolbarSize; i++)
			{
				var slot = _uiSlots[i];
				slot.Initialize(i, _toolbar[i]);
				slot.OnPressed += OnSlotPressed;
			}

			OnActiveSlotChanged(_toolbar.ActiveSlotIndex);
			_toolbar.OnActiveSlotChanged += OnActiveSlotChanged;
		}

		private void OnActiveSlotChanged(int slotIndex)
		{
			_uiSlots[_previousActiveSlotIndex].SetSlotActive(false);
			_uiSlots[slotIndex].SetSlotActive(true);
			_previousActiveSlotIndex = slotIndex;
		}

		private void OnSlotPressed(int slotIndex)
		{
			_toolbar.ActiveSlotIndex = slotIndex;
		}

		private ref PlayerInventoryComponent GetPlayerInventory()
		{
			var playerInventoryPool = _ecsWorld.GetPool<PlayerInventoryComponent>();
			var playerInventoryFilter = _ecsWorld
				.Filter<PlayerComponent>()
				.Inc<PlayerInventoryComponent>()
				.End();
			foreach(var playerEntity in playerInventoryFilter)
			{
				return ref playerInventoryPool.Get(playerEntity);
			}

			throw new Exception($"{nameof(PlayerInventoryComponent)} not found");
		}
	}
}