using System.Collections;
using TMPro;
using UnityEngine;

namespace _Scripts.TempScripts
{
	public class FPSCounter : MonoBehaviour
	{
		private TextMeshProUGUI _text;

		private void Awake()
		{
			_text = GetComponent<TextMeshProUGUI>();
		}

		private void Start()
		{
			StartCoroutine(UpdateFPS());
		}

		private IEnumerator UpdateFPS() 
		{
			while(true)
			{
				var fps = (int)(1f / Time.unscaledDeltaTime);
				_text.text = $"FPS: {fps}";
				yield return new WaitForSeconds(0.1f);
			}
		}
	}
}
