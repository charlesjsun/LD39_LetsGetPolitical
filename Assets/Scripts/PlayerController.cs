using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour 
{

	public LayerMask collisionMask;

	private PlayerMovement movement;
	private Camera cam;

	private GameManager gm;

	private int maxDisplayedFollowers = 500;

	public float ability1Radius = 5f;
	public float ability2Radius = 10f;
	public float ability2SpeedMultiplier = 3f;

	public ParticleSystem ability1Particle;
	public ParticleSystem ability2Particle;

	public List<FollowerController> followers { get; private set; }

	public GameObject arrowPrefab;
	private GameObject currentArrow;

	private bool isUsingAbility2 = false;

	public float ability1CoolDown = 1f;
	public float ability2CoolDown = 0.5f;

	public float ability1Percent { get; private set; }
	public float ability2Percent { get; private set; }

	private void Start()
	{
		followers = new List<FollowerController>();
		movement = GetComponent<PlayerMovement>();
		gm = FindObjectOfType<GameManager>();
		cam = Camera.main;

		ability1Percent = 1f;
		ability2Percent = 1f;
	}
	
	private void Update()
	{
		if (gm.HasGameEnded() || gm.IsGamePaused())
		{
			return;
		}

		ability1Percent += Time.deltaTime / ability1CoolDown;
		ability2Percent += Time.deltaTime / ability2CoolDown;
		ability1Percent = Mathf.Min(ability1Percent, 1f);
		ability2Percent = Mathf.Min(ability2Percent, 1f);

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			//Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 60f);

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, collisionMask))
			{
				//print(hit.collider);
				if (!isUsingAbility2)
				{
					movement.MoveTo(hit.point);
					MakeArrowAt(hit.point);
				}
				else
				{
					UseAbility2AfterClick(hit.point);
				}
			}
		}

		if (Input.GetButtonDown("Ability1"))
		{
			UseAbility1();
		}

		if (Input.GetButtonDown("Ability2"))
		{
			UseAbility2();
		}

		if (currentArrow != null)
		{
			if ((currentArrow.transform.position - transform.position).sqrMagnitude < 2f)
			{
				Destroy(currentArrow);
			}
		}

		//if (Input.GetKeyDown(KeyCode.Alpha3))
		//{
		//	UseAbility3();
		//}

	}

	public bool IsUsingAbility2()
	{
		return isUsingAbility2;
	}

	public void MakeArrowAt(Vector3 location)
	{
		Quaternion rot = Quaternion.identity;
		if (currentArrow != null)
		{
			rot = currentArrow.transform.rotation;
			location.y = currentArrow.transform.position.y;
			Destroy(currentArrow);
		}
		currentArrow = Instantiate(arrowPrefab, location, rot) as GameObject;
	}

	public void LetAPersonLeave()
	{
		if (followers.Count > 0 && gm.GetNumFollowing() <= followers.Count)
		{
			for (int i = 0; i < followers.Count; i++)
			{
				if (followers[i].state == FollowerController.FollowerState.Following)
				{
					followers[i].StartToLeave();
					followers.RemoveAt(i);
					return;
				}
			}
			for (int i = 0; i < followers.Count; i++)
			{
				if (followers[i].state == FollowerController.FollowerState.Ability2)
				{
					followers[i].StartToLeave();
					followers.RemoveAt(i);
					return;
				}
			}
		}
	}

	public void SpawnAbility1Particle(Vector3 position)
	{
		Destroy(Instantiate(ability1Particle, position + Vector3.up, ability1Particle.transform.rotation).gameObject, ability1Particle.main.startLifetime.constant);

	}

	public void SpawnAbility2Particle(Vector3 position)
	{
		Destroy(Instantiate(ability2Particle, position + Vector3.up, ability2Particle.transform.rotation).gameObject, ability2Particle.main.startLifetime.constant);
	}

	public void ConvertPeople(Vector3 position, float radius, bool isFirstConvert = false)
	{
		Collider[] cols = Physics.OverlapSphere(position, radius);

		if (AudioManager.instance != null)
		{
			AudioManager.instance.PlaySound2D("OnConvert1");
		}

		for (int i = 0; i < cols.Length; i++)
		{
			if (cols[i].tag == "Follower")
			{
				FollowerController fc = cols[i].GetComponent<FollowerController>();
				if (fc.state == FollowerController.FollowerState.Wandering)
				{
					//print("CONVERT " + fc);
					followers.Add(fc);
					fc.Convert(isFirstConvert);

					if (followers.Count > maxDisplayedFollowers)
					{
						for (int j = 0; j < followers.Count; j++)
						{
							if (followers[j].state == FollowerController.FollowerState.Following)
							{
								if (followers[j].currentAb2Arrow != null)
								{
									Destroy(followers[j].currentAb2Arrow);
								}
								Destroy(followers[j].gameObject);
								followers.RemoveAt(j);
								break;
							}
						}
					}
					if (followers.Count > maxDisplayedFollowers)
					{ 
						for (int j = 0; j < followers.Count; j++)
						{
							if (followers[j].state == FollowerController.FollowerState.Ability2)
							{
								if (followers[j].currentAb2Arrow != null)
								{
									Destroy(followers[j].currentAb2Arrow);
								}
								Destroy(followers[j].gameObject);
								followers.RemoveAt(j);
								return;
							}
						}
					}

					

				}
			}
		}

	}

	public void UseAbility1()
	{
		if (ability1Percent >= 1f)
		{
			ConvertPeople(transform.position, ability1Radius);
			SpawnAbility1Particle(transform.position);
			ability1Percent = 0f;
		}
	}

	private void UseAbility2AfterClick(Vector3 position)
	{
		FollowerController fc = followers[Random.Range(0, followers.Count)];
		if (fc.state == FollowerController.FollowerState.Ability2)
		{
			bool done = false;
			for (int i = 0; i < followers.Count; i++)
			{
				if (followers[i].state == FollowerController.FollowerState.Following)
				{
					followers[i].DoAbility2(position, ability2Radius, ability2SpeedMultiplier);
					done = true;
					break;
				}
			}
			if (!done)
			{
				print("CANNOT DO ABILITY 2");
			}
		}
		else
		{
			fc.DoAbility2(position, ability2Radius, ability2SpeedMultiplier);
		}
		isUsingAbility2 = false;
		ability2Percent = 0f;
	}

	public void UseAbility2()
	{
		if (ability2Percent >= 1f)
		{
			isUsingAbility2 = !isUsingAbility2;
		}
	}

	//public void UseAbility3()
	//{
	//	for (int i = 0; i < followers.Count; i++)
	//	{
	//		ConvertPeople(followers[i].transform.position, ability3Radius);
	//		SpawnAbilityParticle(followers[i].transform.position, ability3Radius);
	//	}
	//}
	
}
