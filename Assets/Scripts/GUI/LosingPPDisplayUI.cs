using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosingPPDisplayUI : MonoBehaviour 
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
		text.text = "-" + gm.GetPPLostPerSec().ToString("F2") + " PP/s";
	}

}
