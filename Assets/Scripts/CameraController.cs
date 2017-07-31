using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{

	private Transform player;
	private Vector3 offset;

	private float height;

	private Camera cam;

	public float yawSpeed;
	private float yaw = 0;

	private float zoom = 7.5f;
	public float minZoom = 1;
	public float maxZoom = 25f;
	public float zoomSpeed = 1f;

	private void Start()
	{
		player = FindObjectOfType<PlayerController>().transform;
		cam = Camera.main;

		height = player.GetComponent<CapsuleCollider>().height * 3f / 4f;

		offset = new Vector3(1f * Mathf.Sqrt(6f), 2f, 1f * Mathf.Sqrt(6f));
	}

	private void Update()
	{
		if (Input.GetMouseButton(1))
		{
			yaw += Input.GetAxis("Mouse X") * yawSpeed;
		}

		if (Input.GetButtonDown("ZoomIn"))
		{
			zoom -= 2f;
			zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
		}
		
		if (Input.GetButtonDown("ZoomOut"))
		{
			zoom += 2f;
			zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
		}

		zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
		zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

	}
	
	private void LateUpdate()
	{
		transform.position = new Vector3(player.position.x, height, player.position.z) + offset * Mathf.Max(zoom, 4f);
		transform.LookAt(new Vector3(player.position.x, height, player.position.z));
		transform.RotateAround(new Vector3(player.position.x, height, player.position.z), Vector3.up, yaw);

		cam.orthographicSize = zoom;

	}
	
}
