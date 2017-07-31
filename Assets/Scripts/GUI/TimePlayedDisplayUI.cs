using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePlayedDisplayUI : MonoBehaviour 
{

	private Text text;
	private GameManager gm;

	private void Start()
	{
		text = GetComponent<Text>();
		gm = FindObjectOfType<GameManager>();
	}

	private void Update()
	{
		float timePlayed = gm.GetTimePlayed();
		int minutesPlayed = Mathf.FloorToInt(timePlayed / 60);
		int secPlayed = (int)timePlayed % 60;
		text.text = minutesPlayed + " min " + secPlayed + " s";
	}


}
