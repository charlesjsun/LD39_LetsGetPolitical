using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour 
{

	public enum AudioChannel { Master, Sfx, Music }

	private enum MusicType { Menu, Game, None }

	private float masterVol = 1f;
	private float sfxVol = 1f;
	private float musicVol = 1f;

	private AudioSource[] musicSources;
	private int currMusicSrcIndex = 0;
	private MusicType currMusicType = MusicType.None;

	private AudioSource sfx2DSource;

	private AudioListener audioListener;
	private PlayerController player;

	public AudioLibrary audioLibrary { get; private set; }

	public static AudioManager instance { get; private set; }

	private void Start()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);

			musicSources = new AudioSource[2];
			for (int i = 0; i < 2; i++)
			{
				GameObject newMusicSrc = new GameObject("MusicSource" + i);
				musicSources[i] = newMusicSrc.AddComponent<AudioSource>();
				newMusicSrc.transform.parent = transform;
			}

			GameObject newSfx2DSrc = new GameObject("Sfx2DSource");
			sfx2DSource = newSfx2DSrc.AddComponent<AudioSource>();
			sfx2DSource.transform.parent = transform;

			audioListener = FindObjectOfType<AudioListener>();
			player = FindObjectOfType<PlayerController>();

			audioLibrary = GetComponent<AudioLibrary>();
		}
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (player == null)
		{
			player = FindObjectOfType<PlayerController>();
		}
	}
	
	private void Update()
	{

		if (player != null)
		{
			audioListener.transform.position = player.transform.position;
		}

		if (currMusicType == MusicType.Game)
		{
			if (!musicSources[currMusicSrcIndex].isPlaying)
			{
				print("Play New Game Music");
				PlayGameMusic();
			}
		}
		else if (currMusicType == MusicType.Menu)
		{
			if (!musicSources[currMusicSrcIndex].isPlaying)
			{
				print("Play New Menu Music");
				PlayMenuMusic();
			}
		}

	}

	public void PlaySound(AudioClip clip, Vector3 pos)
	{
		AudioSource.PlayClipAtPoint(clip, pos, masterVol * sfxVol);
	}

	public void PlaySound(string name, Vector3 pos)
	{
		PlaySound(audioLibrary.GetClip(name), pos);
	}

	public void PlaySound2D(AudioClip clip)
	{
		sfx2DSource.PlayOneShot(clip, masterVol * sfxVol);
	}

	public void PlaySound2D(string name)
	{
		PlaySound2D(audioLibrary.GetClip(name));
	}

	public void PlayMusic(AudioClip clip, float duration = 1f)
	{
		currMusicSrcIndex = 1 - currMusicSrcIndex;
		musicSources[currMusicSrcIndex].clip = clip;
		musicSources[currMusicSrcIndex].Play();

		StartCoroutine(CrossFadeMusic(duration));
	}

	public void PlayMusic(string name, float duration = 1f)
	{
		PlayMusic(audioLibrary.GetClip(name), duration);
	}

	public void PlayGameMusic(float duration = 1f)
	{
		currMusicType = MusicType.Game;
		PlayMusic(audioLibrary.GetRandomGameClip(), duration);
	}

	public void PlayMenuMusic(float duration = 1f)
	{
		currMusicType = MusicType.Menu;
		PlayMusic(audioLibrary.GetRandomMenuClip(), duration);
	}

	private IEnumerator CrossFadeMusic(float duration)
	{
		float percent = 0f;
		while (percent < 1f)
		{
			percent += Time.deltaTime * (1f / duration);

			musicSources[currMusicSrcIndex].volume = Mathf.Lerp(0, musicVol * masterVol, percent);
			musicSources[1 - currMusicSrcIndex].volume = Mathf.Lerp(musicVol * masterVol, 0, percent);

			yield return null;
		}

		musicSources[1 - currMusicSrcIndex].Stop();
	}
	
}
