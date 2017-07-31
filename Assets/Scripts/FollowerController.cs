using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerController : MonoBehaviour
{

	public enum FollowerState { Wandering, Following, Ability2, Leaving }

	public FollowerState state { get; private set; }

	private PlayerController player;
	private FollowerMovement movement;
	private FollowerGraphics graphics;
	private CapsuleCollider capCollider;

	private NavMeshAgent navAgent;

	public ParticleSystem convertParticles;
	public ParticleSystem leaveParticles;

	public GameObject ability2ArrowPrefab;
	public GameObject currentAb2Arrow;

	

	public event System.Action<FollowerController> OnConvert;

	private void Start()
	{
		state = FollowerState.Wandering;

		movement = GetComponent<FollowerMovement>();
		player = FindObjectOfType<PlayerController>();
		navAgent = GetComponent<NavMeshAgent>();
		graphics = GetComponentInChildren<FollowerGraphics>();
		capCollider = GetComponent<CapsuleCollider>();

		StartCoroutine(UpdateWanderingPath());
	}

	private void Update()
	{

	}

	private bool GetRandomPosition(Vector3 center, float range, out Vector3 result)
	{
		for (int i = 0; i < 30; i++)
		{
			Vector2 point2D = Random.insideUnitCircle;
			Vector3 randomPoint = center + new Vector3(point2D.x, 0, point2D.y) * range;
			NavMeshHit hit;
			if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
			{
				result = hit.position;
				return true;
			}
		}
		result = Vector3.zero;
		return false;
	}

	private IEnumerator UpdateWanderingPath()
	{
		float refreshRateMin = 2f;
		float refreshRateMax = 8f;

		float wanderRange = 64f;

		yield return null;

		while (player != null && state == FollowerState.Wandering)
		{
			Vector3 targetPos;
			if (GetRandomPosition(transform.position, wanderRange, out targetPos))
			{
				movement.MoveTo(new Vector3(targetPos.x, 0, targetPos.z));
			}

			yield return new WaitForSeconds(Random.Range(refreshRateMin, refreshRateMax));
		}
	}

	public void Convert(bool initialConvert = false)
	{
		if (state == FollowerState.Wandering)
		{
			Destroy(Instantiate(convertParticles, transform.position, convertParticles.transform.rotation).gameObject, convertParticles.main.startLifetime.constant);
			graphics.SetHatVisibility(true);
			StartFollowing();

			if (!initialConvert)
			{
				AudioManager.instance.PlaySound2D("OnConvert2");
			}

			if (OnConvert != null)
			{
				OnConvert(this);
			}

		}
		else
		{
			Debug.LogError("Convert called on already converted follower!!!");
		}
	}

	private void StartFollowing()
	{
		state = FollowerState.Following;
		StartCoroutine(UpdateFollowPath());
	}

	private IEnumerator UpdateFollowPath()
	{
		float refreshRate = 1f;

		while (player != null && state == FollowerState.Following)
		{
			Vector3 targetPos = player.transform.position;
			movement.MoveTo(new Vector3(targetPos.x, 0, targetPos.z));
			yield return new WaitForSeconds(refreshRate);
		}

	}

	public void StartToLeave()
	{
		state = FollowerState.Leaving;

		if (currentAb2Arrow != null)
		{
			Destroy(currentAb2Arrow);
		}

		AudioManager.instance.PlaySound2D("FollowerLeave");

		Destroy(Instantiate(leaveParticles, transform.position, leaveParticles.transform.rotation).gameObject, leaveParticles.main.startLifetime.constant);
		Destroy(gameObject);
	}

	public void DoAbility2(Vector3 dest, float radius, float speedMul)
	{
		state = FollowerState.Ability2;

		StartCoroutine(StartAbility2(dest, radius, speedMul));
	}

	private IEnumerator StartAbility2(Vector3 dest, float radius, float speedMul)
	{
		float stoppingDist = navAgent.stoppingDistance;
		float originalSpeed = navAgent.speed;
		navAgent.stoppingDistance = 0f;
		navAgent.speed = originalSpeed * speedMul;
		navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

		capCollider.enabled = false;

		currentAb2Arrow = Instantiate(ability2ArrowPrefab, dest, Quaternion.identity) as GameObject;

		movement.MoveTo(dest);

		yield return null;

		while ((transform.position - navAgent.pathEndPosition).sqrMagnitude > 1f)
		{
			yield return null;
		}

		player.ConvertPeople(navAgent.pathEndPosition, radius);
		player.SpawnAbility2Particle(navAgent.pathEndPosition);

		navAgent.stoppingDistance = stoppingDist;
		navAgent.speed = originalSpeed;
		navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;

		capCollider.enabled = true;

		Destroy(currentAb2Arrow);

		StartFollowing();
	}

	

}
