using System.Collections;
using TMPro;
using UnityEngine;

namespace _Scripts.TempScripts
{
	public class FpsCounter : MonoBehaviour
	{
		private int _previousFps;
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
			var waitForSeconds = new WaitForSeconds(0.1f);
			while(true)
			{
				var fps = (int)(1f / Time.unscaledDeltaTime);
				if(fps != _previousFps)
				{
					_text.text = $"FPS: {fps}";
					_previousFps = fps;
				}

				yield return waitForSeconds;
			}
		}
	}
}
