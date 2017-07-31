using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerGraphics : MonoBehaviour 
{


	private void Start()
	{
		GetComponent<MeshFilter>().mesh = FollowerMaterialManager.instance.GetRandomMesh();
		GetComponent<MeshRenderer>().material = FollowerMaterialManager.instance.GetRandomMaterial();
	}
	
	public void SetHatVisibility(bool visibility)
	{
		transform.GetChild(0).gameObject.SetActive(visibility);
	}
	
}
