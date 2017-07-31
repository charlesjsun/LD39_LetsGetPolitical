using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour 
{

	public float height = 0.75f;
	public float rotateSpeed = 180f;
	public float period = 1f;

	private void Start()
	{
		
	}
	
	private void Update()
	{
		transform.position = new Vector3(transform.position.x, (Mathf.Sin(Time.time * (2f * Mathf.PI / period)) + 1f) * height, transform.position.z);
		transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
	}
	
}
