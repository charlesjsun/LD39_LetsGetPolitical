using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability2Cooldown : MonoBehaviour 
{

	private Slider slider;
	private PlayerController player;

	private Image fillArea;

	public Color normalColor;
	public Color usingColor;

	private void Start()
	{
		slider = GetComponent<Slider>();
		player = FindObjectOfType<PlayerController>();
		fillArea = GetComponentInChildren<Image>();
	}

	private void Update()
	{
		slider.value = player.ability2Percent;

		if (!player.IsUsingAbility2())
		{
			fillArea.color = normalColor;
		}
		else
		{
			fillArea.color = usingColor;
		}

	}

}
