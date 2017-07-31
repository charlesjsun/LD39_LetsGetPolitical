using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerSpawner : MonoBehaviour 
{

	public GameObject followerPrefab;

	public float mapHalfWidth = 102f;

	public int maxWanderFollowers = 100;
	public int numStartFollowers = 10;
	private int currWanderFollowers = 0;

	public float spawnInterval = 5f;
	private float nextSpawn = 0f;

	private GameManager gm;
	private PlayerController player;

	private void Start()
	{
		gm = FindObjectOfType<GameManager>();
		player = FindObjectOfType<PlayerController>();

		for (int i = 0; i < 10; i++)
		{
			Vector2 r = Random.insideUnitCircle;
			Vector3 rad = new Vector3(r.x, 0, r.y);
			NavMeshHit hit;
			NavMesh.SamplePosition(player.transform.position + rad, out hit, 1.0f, NavMesh.AllAreas);
			SpawnNewFollower(hit.position);
		}
	}

	private bool isFirstUpdate = true;

	private void Update()
	{
		if (isFirstUpdate)
		{
			player.ConvertPeople(player.transform.position, 5f, true);
			isFirstUpdate = false;

			for (int i = 0; i < numStartFollowers; i++)
			{
				SpawnRandomFollower();
			}
		}

		if (Time.time >= nextSpawn)
		{
			if (currWanderFollowers < maxWanderFollowers)
			{
				SpawnRandomFollower();
				nextSpawn = Time.time + spawnInterval;
			}
		}
	}

	private void SpawnRandomFollower()
	{
		Vector3 position = Vector3.zero;
		bool done = false;
		for (int i = 0; i < 30; i++)
		{
			Vector3 randomPoint = new Vector3(Random.Range(-mapHalfWidth, mapHalfWidth), 0, Random.Range(-mapHalfWidth, mapHalfWidth));

			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
			{
				position = hit.position;
				done = true;
				break;
			}
		}

		if (!done)
		{
			NavMeshHit hit;
			NavMesh.SamplePosition(Vector3.zero, out hit, 20.0f, NavMesh.AllAreas);
			position = hit.position;
		}

		SpawnNewFollower(position);
	}

	private void SpawnNewFollower(Vector3 position)
	{
		GameObject newFollower = Instantiate(followerPrefab, position, Quaternion.identity) as GameObject;
		FollowerController fc = newFollower.GetComponent<FollowerController>();
		currWanderFollowers++;
		fc.OnConvert += OnFollowerConvert;
		fc.OnConvert += gm.OnFollowerConvert;

		//print("Spawned Follower #" + currWanderFollowers + " at " + position);
	}

	private void OnFollowerConvert(FollowerController fc)
	{
		fc.OnConvert -= OnFollowerConvert;
		currWanderFollowers--;
	}
	
}
