using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour 
{

	public GameObject mainMenu;
	public GameObject helpMenu;
	public GameObject creditsMenu;

	private bool isFirstFrame = true;

	private void Start()
	{
		
	}

	private void Update()
	{
		if (isFirstFrame)
		{
			AudioManager.instance.PlayMenuMusic();
			isFirstFrame = false;
		}
	}
	
	public void OnPlay()
	{
		SceneManager.LoadScene("Game");
		AudioManager.instance.PlayGameMusic(2f);
	}

	public void OnHelp()
	{
		mainMenu.SetActive(false);
		helpMenu.SetActive(true);
	}

	public void OnCredits()
	{
		mainMenu.SetActive(false);
		creditsMenu.SetActive(true);
	}

	public void OnQuit()
	{
		Application.Quit();
	}

	public void OnBack()
	{
		mainMenu.SetActive(true);
		creditsMenu.SetActive(false);
		helpMenu.SetActive(false);
	}
	
}
