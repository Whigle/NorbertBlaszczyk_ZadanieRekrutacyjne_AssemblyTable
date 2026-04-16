using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SystemElementSpawner : MonoBehaviour
{
	public static SystemElementSpawner Instance;

	[SerializeField]
	private Transform spawnPoint;

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
		var handle = data.Prefab.InstantiateAsync(spawnPoint.position, Quaternion.identity);

		GameObject gameObject = await handle.Task;

		gameObject.GetComponent<SystemElement>()?.Init(data);
	}

	private void OnDrawGizmos()
	{
		if(spawnPoint != null) {
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(spawnPoint.transform.position, .25f);
		}
	}
}
