using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour 
{

	private Image background;

	private void Start()
	{
		background = GetComponent<Image>();
		StartCoroutine(Fade(new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 0.7f), 1f));
	}
	
	private IEnumerator Fade(Color from, Color to, float time)
	{
		float speed = 1f / time;
		float percent = 0;

		while (percent < 1)
		{
			percent += Time.deltaTime * speed;
			background.color = Color.Lerp(from, to, percent);
			yield return null;
		}
	}


}
