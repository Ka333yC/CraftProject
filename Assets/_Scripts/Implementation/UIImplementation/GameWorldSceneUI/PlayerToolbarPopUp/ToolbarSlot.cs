using System;
using _Scripts.Core.InventoryCore.SlotLogic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Implementation.UIImplementation.GameWorldSceneUI.PlayerToolbarPopUp
{
	public class ToolbarSlot : MonoBehaviour
	{
		[SerializeField] 
		private Image _itemImage;
		// [SerializeField] 
		// private TextMeshProUGUI _itemNameText;
		[SerializeField] 
		private TextMeshProUGUI _countText;
		[SerializeField] 
		private Button _onSlotButton;
		[SerializeField] 
		private GameObject _slotActiveFrame;
		
		private int _slotIndex;
		private InventorySlot _slot;
		
		public event Action<int> OnPressed;

		private void Awake()
		{
			SetSlotActive(false);
			_onSlotButton.onClick.AddListener(() => OnPressed?.Invoke(_slotIndex));
		}
		
		public void Initialize(int slotIndex, InventorySlot slot)
		{
			_slotIndex = slotIndex;
			_slot = slot;
			_slot.OnItemChanged += OnItemChanged;
			OnItemChanged();
		}

		public void SetSlotActive(bool isActive) 
		{
			_slotActiveFrame.gameObject.SetActive(isActive);
		}

		private void OnItemChanged()
		{
			if(!_slot.HasItem)
			{
				_itemImage.gameObject.SetActive(false);
				_countText.gameObject.SetActive(false);
				return;
			}

			var item = _slot.Item;
			_itemImage.sprite = item.ItemData.Icon;
			_itemImage.gameObject.SetActive(true);
			_countText.text = item.Count.ToString();
			_countText.gameObject.SetActive(item.Count != 1);
		}
	}
}