using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability1Cooldown : MonoBehaviour 
{

	private Slider slider;
	private PlayerController player;

	private void Start()
	{
		slider = GetComponent<Slider>();
		player = FindObjectOfType<PlayerController>();
	}
	
	private void Update()
	{
		slider.value = player.ability1Percent;
	}
	
}
