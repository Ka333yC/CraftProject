using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;

namespace Assets.Scripts.Core.InputCore
{
	public class OnScreenButton : OnScreenControl, IPointerDownHandler, IPointerUpHandler
	{
		[InputControl(layout = "Button")]
		[SerializeField]
		private string m_ControlPath;

		private Vector2? _pointerPressPosition;

		protected override string controlPathInternal
		{
			get
			{
				return m_ControlPath;
			}

			set
			{
				m_ControlPath = value;
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			if(_pointerPressPosition.HasValue)
			{
				return;
			}

			_pointerPressPosition = eventData.pressPosition;
			SendValueToControl(1f);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if(!_pointerPressPosition.HasValue ||
				!Mathf.Approximately((_pointerPressPosition.Value - eventData.pressPosition).magnitude, 0))
			{
				return;
			}

			_pointerPressPosition = null;
			SendValueToControl(0f);
		}
	}
}
