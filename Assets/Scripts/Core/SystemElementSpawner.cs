using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemElementSpawner : MonoBehaviour
{
	public static SystemElementSpawner Instance;

	private void Awake()
	{
		if(Instance != null) {
			Debug.LogError("More then one SystemElementSpawner on scene, this one will be destroyed", this);
			Destroy(this);
		}

		Instance = this;
	}

	public async void SpawnSystemElement(SystemElementSO data) 
	{
		var handle = data.Prefab.InstantiateAsync(Vector3.zero, Quaternion.identity);

		GameObject gameObject = await handle.Task;

		gameObject.GetComponent<SystemElement>()?.Init(data);
	}
}
