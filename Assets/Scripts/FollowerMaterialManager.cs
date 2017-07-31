using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMaterialManager : MonoBehaviour 
{

	public Mesh[] meshes;
	public Material[] materials;

	public static FollowerMaterialManager instance { get; private set; }

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
	}

	public Mesh GetRandomMesh()
	{
		return meshes[Random.Range(0, meshes.Length)];
	}

	public Material GetRandomMaterial()
	{
		return materials[Random.Range(0, materials.Length)];
	}

}
