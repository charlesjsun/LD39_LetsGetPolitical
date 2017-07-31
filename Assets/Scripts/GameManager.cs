using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{

	private PlayerController player;

	private int numFollowing = 0;
	private float politicalPower = 0f;
	private float ppLostPerSec = 0.1f;
	private float ppLostIncreasePerSec = 0.001f;
	private float ppLostIncreaseIncrease = 0.001f;

	private float timePlayed = 0f;
	private int highestPP = 0;

	private bool hasGameEnded = false;
	private bool isGamePaused = false;

	public GameObject gui;
	public GameObject gameOverScreen;
	public GameObject pauseScreen;

	private void Start()
	{
		numFollowing = 0;
		player = FindObjectOfType<PlayerController>();
	}
	
	private void Update()
	{
		if (!hasGameEnded)
		{
			if (!isGamePaused)
			{
				timePlayed += Time.deltaTime;

				highestPP = Mathf.Max(highestPP, numFollowing);

				politicalPower -= ppLostPerSec * Time.deltaTime;

				if (politicalPower > 225f || timePlayed > 3f * 60f)
				{
					ppLostIncreasePerSec += ppLostIncreaseIncrease * Time.deltaTime;
				}

				ppLostPerSec += ppLostIncreasePerSec * Time.deltaTime;


				if (numFollowing - politicalPower >= 1f)
				{
					int numToLeave = Mathf.FloorToInt(numFollowing - politicalPower);
					for (int i = 0; i < numToLeave; i++)
					{
						player.LetAPersonLeave();
						numFollowing--;

						if (numFollowing <= 0)
						{
							EndGame();
						}
					}
				}
			}

			if (Input.GetButtonDown("Pause"))
			{
				if (isGamePaused)
				{
					UnpauseGame();
				}
				else
				{
					PauseGame();
				}
			}
		}
	}

	public void PauseGame()
	{
		isGamePaused = true;
		Time.timeScale = 0f;
		pauseScreen.SetActive(true);
	}

	public void UnpauseGame()
	{
		isGamePaused = false;
		Time.timeScale = 1f;
		pauseScreen.SetActive(false);
	}

	public void EndGameFromPause()
	{
		if (isGamePaused)
		{
			UnpauseGame();
		}
		pauseScreen.SetActive(false);
		EndGame();
	}

	private void EndGame()
	{
		Debug.Log("GAME ENDED");
		hasGameEnded = true;
		gui.SetActive(false);
		gameOverScreen.SetActive(true);
		AudioManager.instance.PlaySound2D("GameOver");
	}

	public void ReturnToMenu()
	{
		AudioManager.instance.PlayMenuMusic(2f);
		SceneManager.LoadScene("Menu");
	}

	public void OnFollowerConvert(FollowerController fc)
	{
		fc.OnConvert -= OnFollowerConvert;
		numFollowing++;
		politicalPower += 1f;
	}

	public int GetNumFollowing()
	{
		return numFollowing;
	}

	public float GetPPLostPerSec()
	{
		return ppLostPerSec;
	}

	public float GetNextPPLossTime()
	{
		float rp = 1f - (numFollowing - politicalPower);
		float sec = rp * (1f / ppLostPerSec);
		return sec;
	}

	public float GetTimePlayed()
	{
		return timePlayed;
	}

	public int getHighestPP()
	{
		return highestPP;
	}

	public bool HasGameEnded()
	{
		return hasGameEnded;
	}

	public bool IsGamePaused()
	{
		return isGamePaused;
	}
	
}
