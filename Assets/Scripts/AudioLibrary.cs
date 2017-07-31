using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : MonoBehaviour 
{

	[System.Serializable]
	public class AudioPair
	{
		public string name;
		public AudioClip clip;
	}

	public AudioPair[] audioPairs; 
	private Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();

	public AudioClip[] menuMusic;
	public AudioClip[] gameMusic;

	private void Start()
	{
		foreach (AudioPair pair in audioPairs)
		{
			audioDictionary.Add(pair.name, pair.clip);
		}
	}
	
	private void Update()
	{
		
	}

	public AudioClip GetClip(string name)
	{
		if (audioDictionary.ContainsKey(name))
		{
			return audioDictionary[name];
		}
		return null;
	}

	public AudioClip GetRandomMenuClip()
	{
		return menuMusic[Random.Range(0, menuMusic.Length)];
	}

	public AudioClip GetRandomGameClip()
	{
		return gameMusic[Random.Range(0, gameMusic.Length)];
	}

}
