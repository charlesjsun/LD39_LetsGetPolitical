using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerMovement : MonoBehaviour
{

	private NavMeshAgent navAgent;

	private bool isGrounded = true;
	private bool isMoving = false;

	public float jumpTime = 0.25f;
	public float maxJumpHeight = 0.5f;

	private float currentJumpHeight = 0f;

	private Vector3 prevPos;

	private Vector3 navAgentNexPos;

	private bool isAgentPaused = false;

	private void Start()
	{
		navAgent = GetComponent<NavMeshAgent>();
		navAgent.updatePosition = false;

		//rb = GetComponent<Rigidbody>();

		navAgentNexPos = transform.position;
		prevPos = transform.position;
	}

	private void Update()
	{
		if (!isAgentPaused)
		{
			navAgentNexPos = navAgent.nextPosition;

			Vector3 deltaPos = transform.position - prevPos;
			deltaPos.y = 0;
			float deltaDist = deltaPos.sqrMagnitude;

			prevPos = transform.position;

			//if (navAgentNexPos == navAgent.pathEndPosition)
			if (deltaDist < 0.0001f)
			{
				isMoving = false;
			}
			else
			{
				isMoving = true;
			}
		}
	}

	private void PauseNavAgent()
	{
		isAgentPaused = true;
		navAgent.enabled = false;
		isMoving = false;
	}

	private void UnPauseNavAgent()
	{
		isAgentPaused = false;
		navAgent.enabled = true;
		isMoving = true;
	}

	private void FixedUpdate()
	{
		//rb.MovePosition(new Vector3(navAgentNexPos.x, currentJumpHeight, navAgentNexPos.z));
		transform.position = new Vector3(navAgentNexPos.x, currentJumpHeight, navAgentNexPos.z);

		if (isGrounded && isMoving)
		{
			StartCoroutine(Jump());
		}
	}

	private IEnumerator Jump()
	{
		isGrounded = false;

		float percent = 0f;
		while (percent < 1f)
		{
			percent += Time.deltaTime * 1f / jumpTime;

			float lerpVal = (-percent * percent + percent) * 4f;
			currentJumpHeight = Mathf.Lerp(0, maxJumpHeight, lerpVal);

			yield return null;
		}

		isGrounded = true;
	}

	public void MoveTo(Vector3 pos)
	{
		if (navAgent.enabled)
		{
			navAgent.SetDestination(pos);
		}
	}

}
