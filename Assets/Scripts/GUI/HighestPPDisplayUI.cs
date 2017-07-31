using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighestPPDisplayUI : MonoBehaviour 
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
		text.text = gm.getHighestPP().ToString();
	}

}
